using Common.BinaryFlags;
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
internal sealed class PeopleCommandHandler : IPeopleCommandHandler
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
        { //how to add an event for this as there is no aggregate root? Could let the repository take in orphan events and put them in the context. The question thus become how to retrieve them?
            _unitOfWork.AddOrphanEvent(new PersonHiredFailed(result.Errors, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return new InvalidResultNoData(result.Errors); //or maybe via the unit of work?
        }
        _unitOfWork.PersonRepository.Hire(result.Data);
        result.Data.AddDomainEvent(new PersonHiredSucceeded(result.Data, result.Data.Events.Count(), command.CorrelationId, command.CommandId));
        _unitOfWork.Save();
        return new SuccessResultNoData();
    } 

    public Result Handle(FirePersonFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.FiredFrom.Year, command.FiredFrom.Month, command.FiredFrom.Day));
            entity.AddDomainEvent(new PersonFiredSucceeded(entity, entity.Events.Count(), command.CorrelationId, command.CommandId)); //the event should first be triggered when DeletedFrom is true
            _unitOfWork.PersonRepository.Fire(entity); //could store the event in the context and let a process run through events at times to see which needs processing
            //also need to create an integration event, which again should first be processed when the fired from date is current or passed.
        }
        else
        {
            _unitOfWork.AddOrphanEvent(new PersonFiredFailed(new string[] { "Not found." }, command.CorrelationId, command.CommandId));
        }
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonalInformationFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        { //for any command that is triggered by an event should cause an expection as entity is null should not happen if the first command handler is implemented correctly.
            _unitOfWork.AddOrphanEvent(new PersonPersonalInformationChangedFailed(new string[] { "Not found." }, 0, command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); //consider calling something else like 'ProcessEvents'
            return new InvalidResultNoData("Not found"); //what to do with create commands? there are no aggregate roots if they fail. Maybe have event ctor with aggregate id = 0 and aggregatetype 'hardcoded' typeof(Person).name 
        }

        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        BinaryFlag flag = new PersonChangePersonalInformationValidator(command, validationData).Validate();
        if (!flag)
        { //have event
            entity.AddDomainEvent(new PersonPersonalInformationChangedFailed(entity, PersonErrorConversion.Convert(flag), entity.Events.Count(),  command.CorrelationId, command.CommandId));
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
            entity.AddDomainEvent(new PersonChangedGender(entity, oldGender.Id, entity.Events.Count(), command.CorrelationId, command.CommandId));
        }

        var firstNameChanged = command.FirstName is not null;
        var lastNameChanged = command.LastName is not null;
        var birthChanged = command.Brith is not null;
        var genderChanged = command.Gender is not null;
        entity.AddDomainEvent(new PersonPersonalInformationChangedSuccessed(entity, entity.Events.Count(), command.CorrelationId, command.CommandId, firstNameChanged, lastNameChanged, birthChanged, genderChanged));
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
            _unitOfWork.AddOrphanEvent(new GenderRecognisedFailed(result.Errors, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return new InvalidResultNoData(result.Errors);
        }
        result.Data.AddDomainEvent(new GenderRecognisedSucceeded(result.Data, result.Data.Events.Count(), command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Recognise(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddPersonToGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result; //events should not really return data. If something goes wrong another event can be created to inform about this (so sagas and such)
        if (entity is null)
        {
            _unitOfWork.AddOrphanEvent(new PersonAddedToGenderFailed(new string[] { $"Gender {command.GenderId} was not found." }, command.CorrelationId, command.CommandId)); ;
            _unitOfWork.Save();
            return new InvalidResultNoData();//create an event for a saga to handle and stop the execution of the current code.
        } //should validate if the person id exist
        entity.AddPerson(new(command.PersonId));
        entity.AddDomainEvent(new PersonAddedToGenderSucceeded(entity, command.PersonId, entity.Events.Count(), command.CorrelationId, command.CommandId));
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
        entity.AddDomainEvent(new PersonRemovedFromGenderSucceeded(entity, command.PersonId, entity.Events.Count(), command.CorrelationId, command.CommandId));
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
        entityToAddToo.AddDomainEvent(new PersonAddedToGenderSucceeded(entityToAddToo, command.PersonId, entityToAddToo.Events.Count(), command.CorrelationId, command.CommandId));
        entityToRemoveFrom.RemovePerson(new(command.PersonId));
        entityToRemoveFrom.AddDomainEvent(new PersonRemovedFromGenderSucceeded(entityToRemoveFrom, command.PersonId, entityToRemoveFrom.Events.Count(), command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entityToAddToo);
        _unitOfWork.GenderRepository.Update(entityToRemoveFrom); //have an event to indicate this is done, mayhaps similar name to the event but with Success at the end?
        return new SuccessResultNoData();
    }

    public Result Handle(UnrecogniseGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            _unitOfWork.AddOrphanEvent(new GenderUnrecognisedFailed(new string[] {"Not Found."}, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return new SuccessResultNoData();
        }
        if (entity.People.Any())
        {
            _unitOfWork.AddOrphanEvent(new GenderUnrecognisedFailed(new string[] { $"People identify with gender {entity.VerbSubject}/{entity.VerbObject}." }, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return new InvalidResultNoData($"People identify with gender {entity.VerbSubject}/{entity.VerbObject}.");
        }
        entity.AddDomainEvent(new GenderUnrecognisedSucceeded(entity, entity.Events.Count(), command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Unrecognise(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }
}
