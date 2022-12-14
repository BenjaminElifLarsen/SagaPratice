using Common.BinaryFlags;
using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.CQRS.Queries;
using PersonDomain.DL.Errrors;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Handlers.Command;
internal sealed class PersonCommandHandler : IPersonCommandHandler
{
    private readonly IPersonFactory _personFactory;
    private readonly IGenderFactory _genderFactory;
    private readonly IUnitOfWork _unitOfWork;

    public PersonCommandHandler(IPersonFactory personFactory, IGenderFactory genderFactory, IUnitOfWork unitOfWork)
    {
        _personFactory = personFactory;
        _genderFactory = genderFactory;
        _unitOfWork = unitOfWork;
    }

    public void Handle(HirePersonFromUser command)
    {
        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        var result = _personFactory.CreatePerson(command, validationData);
        if (result is InvalidResult<Person>)
        { //how to add an event for this as there is no aggregate root? Could let the repository take in orphan events and put them in the context. The question thus become how to retrieve them?
            _unitOfWork.AddSystemEvent(new PersonHiredFailed(result.Errors, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return; // new InvalidResultNoData(result.Errors); //or maybe via the unit of work?
        }
        _unitOfWork.PersonRepository.Hire(result.Data);
        result.Data.AddDomainEvent(new PersonHiredSucceeded(result.Data, command.CorrelationId, command.CommandId));
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    } 

    public void Handle(FirePersonFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.FiredFrom.Year, command.FiredFrom.Month, command.FiredFrom.Day));
            entity.AddDomainEvent(new PersonFiredSucceeded(entity, command.CorrelationId, command.CommandId)); //the event should first be triggered when DeletedFrom is true
            _unitOfWork.PersonRepository.Fire(entity); //could store the event in the context and let a process run through events at times to see which needs processing
            //also need to create an integration event, which again should first be processed when the fired from date is current or passed.
        }
        else
        {
            _unitOfWork.AddSystemEvent(new PersonFiredFailed(new string[] { "Not found." }, command.CorrelationId, command.CommandId));
        }
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(ChangePersonalInformationFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        { //for any command that is triggered by an event should cause an expection as entity is null should not happen if the first command handler is implemented correctly.
            _unitOfWork.AddSystemEvent(new PersonPersonalInformationChangedFailed(new string[] { "Not found." }, command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); //consider calling something else like 'ProcessEvents'
            return; // new InvalidResultNoData("Not found"); //what to do with create commands? there are no aggregate roots if they fail. Maybe have event ctor with aggregate id = 0 and aggregatetype 'hardcoded' typeof(Person).name 
        }

        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        BinaryFlag flag = new PersonChangePersonalInformationValidator(command, validationData).Validate();
        if (!flag)
        { //have event
            _unitOfWork.AddSystemEvent(new PersonPersonalInformationChangedFailed(entity, PersonErrorConversion.Convert(flag), command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); //not really happy with this design. Mayhaps also have a call that permit publishing evnets. Then again if events is going to be saved, maybe it make sense??
            return; // new InvalidResultNoData(PersonErrorConversion.Convert(flag).ToArray());
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
            entity.UpdateGenderIdentification(command.Gender.Gender);
            entity.AddDomainEvent(new PersonChangedGender(entity, oldGender, command.CorrelationId, command.CommandId));
        }

        var firstNameChanged = command.FirstName is not null;
        var lastNameChanged = command.LastName is not null;
        var birthChanged = command.Brith is not null;
        var genderChanged = command.Gender is not null;
        entity.AddDomainEvent(new PersonPersonalInformationChangedSuccessed(entity, firstNameChanged, lastNameChanged, birthChanged, genderChanged, command.CorrelationId, command.CommandId));
        _unitOfWork.PersonRepository.UpdatePersonalInformation(entity);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(RecogniseGender command)
    {
        var genders = _unitOfWork.GenderRepository.AllAsync(new GenderVerbQuery()).Result;
        var validationData = new GenderValidationData(genders);
        var result = _genderFactory.CreateGender(command, validationData);
        if (result is InvalidResult<Gender>)
        {
            _unitOfWork.AddSystemEvent(new GenderRecognisedFailed(result.Errors, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return; // new InvalidResultNoData(result.Errors);
        }
        result.Data.AddDomainEvent(new GenderRecognisedSucceeded(result.Data, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Recognise(result.Data);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }

    public void Handle(AddPersonToGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result; //events should not really return; // data. If something goes wrong another event can be created to inform about this (so sagas and such)
        //var test = _unitOfWork.GenderEventRepository.GetGenderAsync(command.GenderId).Result;
        if (entity is null)
        {
            _unitOfWork.AddSystemEvent(new PersonAddedToGenderFailed(command.PersonId, command.GenderId, new string[] { $"Gender {command.GenderId} was not found." }, command.CorrelationId, command.CommandId)); ;
            _unitOfWork.Save();
            return; // new InvalidResultNoData();//create an event for a saga to handle and stop the execution of the current code.
        } //should validate if the person id exist
        entity.AddPerson(command.PersonId);
        entity.AddDomainEvent(new PersonAddedToGenderSucceeded(entity, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(RemovePersonFromGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result;
        if (entity is null)
        {
            return; // new InvalidResultNoData();//create an event for a saga to handle and stop the execution of the current code.
            //cause an error that will prevent the saving of person.
        } //these handlers should, in theory, not fail as they rely on validated data.
        entity.RemovePerson(command.PersonId); //no need for validating if the person exist or not as they are getting removed.
        entity.AddDomainEvent(new PersonRemovedFromGenderSucceeded(entity, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entity);
        return; // new SuccessResultNoData();
    }

    public void Handle(ChangePersonGender command)
    { //would need to validate if the person is valid, so has set value and exist in the context.
        var entityToAddToo = _unitOfWork.GenderRepository.GetForOperationAsync(command.NewGenderId).Result;
        var entityToRemoveFrom = _unitOfWork.GenderRepository.GetForOperationAsync(command.OldGenderId).Result;
        if (entityToAddToo is null || entityToRemoveFrom is null)
        {
            return; // new InvalidResultNoData(); //create event for saga
        }
        entityToAddToo.AddPerson(command.PersonId);
        entityToAddToo.AddDomainEvent(new PersonAddedToGenderSucceeded(entityToAddToo, command.PersonId, command.CorrelationId, command.CommandId));
        entityToRemoveFrom.RemovePerson(command.PersonId);
        entityToRemoveFrom.AddDomainEvent(new PersonRemovedFromGenderSucceeded(entityToRemoveFrom, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entityToAddToo);
        _unitOfWork.GenderRepository.Update(entityToRemoveFrom); //have an event to indicate this is done, mayhaps similar name to the event but with Success at the end?
        return; // new SuccessResultNoData();
    }

    public void Handle(UnrecogniseGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            _unitOfWork.AddSystemEvent(new GenderUnrecognisedFailed(new string[] {"Not Found."}, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return; // new SuccessResultNoData();
        }
        if (entity.People.Any())
        {
            _unitOfWork.AddSystemEvent(new GenderUnrecognisedFailed(new string[] { $"People identify with gender {entity.VerbSubject}/{entity.VerbObject}." }, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return; // new InvalidResultNoData($"People identify with gender {entity.VerbSubject}/{entity.VerbObject}.");
        }
        entity.AddDomainEvent(new GenderUnrecognisedSucceeded(entity, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Unrecognise(entity);
        _unitOfWork.Save();
        return; // new SuccessResultNoData();
    }
}
