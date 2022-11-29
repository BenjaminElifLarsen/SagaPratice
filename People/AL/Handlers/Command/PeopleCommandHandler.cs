using Common.Other;
using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.CQRS.Queries;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.DL.Factories;
using PeopleDomain.DL.Models;
using PeopleDomain.DL.Validation;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Handlers.Command;
internal class PeopleCommandHandler : IPeopleCommandHandler
{
    private readonly IPersonFactory _personFactory;
    private readonly IGenderFactory _genderFactory;
    private readonly IUnitOfWork _unitOfWork;

    public PeopleCommandHandler(IPersonFactory personFactory, IGenderFactory genderFactory, IUnitOfWork unitOfWork)
    {
        _personFactory = personFactory;
        _genderFactory = genderFactory;
        _unitOfWork = unitOfWork;
    }

    public Result Handle(HirePersonFromUser command)
    {
        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        var result = _personFactory.CreatePerson(command, validationData);
        if (result is InvalidResult<Person>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.PersonRepository.Hire(result.Data);
        try
        {
            result.Data.AddDomainEvent(new PersonHired(result.Data, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        return new SuccessResultNoData();
    } //maybe move all of the handler implementation code into domain services, one for each aggregate roots

    public Result Handle(FirePersonFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.FiredFrom.Year, command.FiredFrom.Month, command.FiredFrom.Day));
            entity.AddDomainEvent(new PersonFired(entity, command.CorrelationId, command.CommandId)); //the event should first be triggered when DeletedFrom is true
            _unitOfWork.PersonRepository.Fire(entity); //could store the event in the context and let a process run through events at times to see which needs processing
            _unitOfWork.Save(); //also need to create an integration event, which again should first be processed when the fired from date is current or passed.
        }
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonalInformationFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        { //for any command that is triggered by an event should cause an expection as entity is null should not happen if the first command handler is implemented correctly.
            return new InvalidResultNoData("Not found");
        }

        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        BinaryFlag flag = new PersonChangePersonalInformationValidator(command, validationData).Validate();
        if (!flag)
        { //have event
            entity.AddDomainEvent(new PersonPersonalInformationChangedFailed(entity, PersonErrorConversion.Convert(flag),  command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); //not really happy with this design. Mayhaps also have a call that permit publishing evnets. Then again if events is going to be saved, maybe it make sense??
            return new InvalidResultNoData(PersonErrorConversion.Convert(flag).ToArray());
        }

        if (command.FirstName is not null)
        {
            entity.ReplaceFistName(command.FirstName.FirstName);
        }
        if (command.LastName is not null)
        {
            entity.ReplaceLastName(command.LastName.LastName);
        }
        if (command.Brith is not null)
        { //integration event
            entity.UpdateBirth(command.Brith.Birth);
        }
        if (command.Gender is not null)
        {
            var oldGender = entity.Gender;
            entity.UpdateGenderIdentification(new(command.Gender.Gender));
            entity.AddDomainEvent(new PersonChangedGender(entity, oldGender.Id, command.CorrelationId, command.CommandId));
        } //maybe best to have a single event for all changes that have multiple handlers
        //it would make it easier for the process manager as it should only care about what has done and need to do next
        //if multiple 'update' events, not all may be used each time, it would need to know the command data
        //then again it would need to know which 'split'-commands succesed or failed and thus need to know which events, at that point, it should wait for as not all commands could be in use
        //so it would just move the problem to a different place. 
        var firstNameChanged = command.FirstName is not null;
        var lastNameChanged = command.LastName is not null;
        var birthChanged = command.Brith is not null;
        var genderChanged = command.Gender is not null;
        entity.AddDomainEvent(new PersonPersonalInformationChangedSuccessed(entity, command.CorrelationId, command.CommandId, firstNameChanged, lastNameChanged, birthChanged, genderChanged));
        _unitOfWork.PersonRepository.UpdatePersonalInformation(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RecogniseGender command)
    {
        var genders = _unitOfWork.GenderRepository.AllAsync(new GenderVerbQuery()).Result;
        var validationData = new GenderValidationData(genders);
        var result = _genderFactory.CreateGender(command, validationData);
        if (result is InvalidResult<Gender>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        result.Data.AddDomainEvent(new GenderRecognised(result.Data, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Recognise(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddPersonToGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result; //events should not really return data. If something goes wrong another event can be created to inform about this (so sagas and such)
        if (entity is null)
        {
            return new InvalidResultNoData();//create an event for a saga to handle and stop the execution of the current code.
        } //should validate if the person id exist
        entity.AddPerson(new(command.PersonId));
        entity.AddDomainEvent(new PersonAddedToGenderSuccessed(entity, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(RemovePersonFromGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData();//create an event for a saga to handle and stop the execution of the current code.
            //cause an error that will prevent the saving of person.
        } //these handlers should, in theory, not fail as they rely on validated data.
        entity.RemovePerson(new(command.PersonId)); //no need for validating if the person exist or not as they are getting removed.
        entity.AddDomainEvent(new PersonRemovedFromGenderSuccessed(entity, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonGender command)
    { //would need to validate if the person is valid, so has set value and exist in the context.
        var entityToAddToo = _unitOfWork.GenderRepository.GetForOperationAsync(command.NewGenderId).Result;
        var entityToRemoveFrom = _unitOfWork.GenderRepository.GetForOperationAsync(command.OldGenderId).Result;
        if (entityToAddToo is null || entityToRemoveFrom is null)
        {
            return new InvalidResultNoData(); //create event for saga
        }
        entityToAddToo.AddPerson(new(command.PersonId));
        entityToRemoveFrom.RemovePerson(new(command.PersonId));
        _unitOfWork.GenderRepository.Update(entityToAddToo);
        _unitOfWork.GenderRepository.Update(entityToRemoveFrom); //have an event to indicate this is done, mayhaps similar name to the event but with Success at the end?
        return new SuccessResultNoData();
    }

    public Result Handle(UnrecogniseGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new SuccessResultNoData();
        }
        if (entity.People.Any())
        {
            return new InvalidResultNoData($"People identify with gender {entity.VerbSubject}/{entity.VerbObject}.");
        }
        entity.AddDomainEvent(new GenderUnrecognised(entity, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Unrecognise(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }
}
