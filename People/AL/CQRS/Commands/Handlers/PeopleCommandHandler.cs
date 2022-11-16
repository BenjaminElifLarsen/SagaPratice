using Common.Events.Domain;
using Common.Other;
using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.CQRS.Queries;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.DL.Factories;
using PeopleDomain.DL.Model;
using PeopleDomain.DL.Validation;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.CQRS.Commands.Handlers;
internal class PeopleCommandHandler : IPeopleCommandHandler
{
    private readonly IPersonFactory _personFactory;
    private readonly IGenderFactory _genderFactory;
    private readonly IDomainEventBus _domainEventBus;
    private readonly IUnitOfWork _unitOfWork;

    public PeopleCommandHandler(IPersonFactory personFactory, IGenderFactory genderFactory, IDomainEventBus domainEventBus, IUnitOfWork unitOfWork)
    {
        _personFactory = personFactory;
        _genderFactory = genderFactory;
        _unitOfWork = unitOfWork;
        _domainEventBus = domainEventBus;
        _domainEventBus.RegisterHandler<PersonHired>(EventAction);
        _domainEventBus.RegisterHandler<PersonFired>(EventAction);
        _domainEventBus.RegisterHandler<PersonChangedGender>(EventAction);
    }

    public void EventAction(PersonHired e)
    {
        Handle(new AddPersonToGender(e.Data.PersonId, e.Data.GenderId));
    }

    public void EventAction(PersonFired e)
    {
        Handle(new RemovePersonFromGender(e.Data.PersonId, e.Data.GenderId));
    }

    public void EventAction(PersonChangedGender e)
    {
        Handle(new ChangePersonGender(e.Data.PersonId, e.Data.NewGenderId, e.Data.OldGenderId));
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
            result.Data.AddDomainEvent(new PersonHired(result.Data));
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
            entity.AddDomainEvent(new PersonFired(entity)); //the event should first be triggered when DeletedFrom is true
            _unitOfWork.PersonRepository.Fire(entity); //could store the event in the context and let a process run through events at times to see which needs processing
            _unitOfWork.Save(); //also need to create an integration event, which again should first be processed when the fired from date is current or passed.
        }
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonalInformationFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData("Not found");
        }

        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        BinaryFlag flag = new PersonChangePersonalInformationValidator(command,validationData).Validate();
        if (!flag)
        {
            return new InvalidResultNoData(PersonErrorConversion.Convert(flag).ToArray());
        }

        if(command.FirstName is not null)
        {
            entity.ReplaceFistName(command.FirstName.FirstName);
        }
        if(command.LastName is not null)
        {
            entity.ReplaceLastName(command.LastName.LastName);
        }
        if(command.Brith is not null)
        { //integration event
            entity.UpdateBirth(command.Brith.Birth);
        }
        if(command.Gender is not null)
        {
            var oldGender = entity.Gender;
            entity.UpdateGenderIdentification(new(command.Gender.Gender));
            entity.AddDomainEvent(new PersonChangedGender(entity, oldGender.Id));
        }

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
        _unitOfWork.GenderRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonGender command)
    { //would need to validate if the person is valid, so has set value and exist in the context.
        var entityToAddToo = _unitOfWork.GenderRepository.GetForOperationAsync(command.NewGenderId).Result;
        var entityToRemoveFrom = _unitOfWork.GenderRepository.GetForOperationAsync(command.OldGenderId).Result;
        if(entityToAddToo is null || entityToRemoveFrom is null)
        {
            return new InvalidResultNoData(); //create event for saga
        }
        entityToAddToo.AddPerson(new(command.PersonId));
        entityToRemoveFrom.RemovePerson(new(command.PersonId));
        _unitOfWork.GenderRepository.Update(entityToAddToo);
        _unitOfWork.GenderRepository.Update(entityToRemoveFrom);
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
        _unitOfWork.GenderRepository.Unrecognise(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }
}
