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
            _unitOfWork.Save(); // TODO: does not make sense to save on failer.
            return; ; //or maybe via the unit of work?
        }
        _unitOfWork.PersonRepository.Hire(result.Data);
        result.Data.AddDomainEvent(new PersonHiredSucceeded(result.Data, command.CorrelationId, command.CommandId));
        _unitOfWork.Save();
        return; 
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
        return; 
    }

    public void Handle(ChangePersonalInformationFromUser command)
    {
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        { 
            _unitOfWork.AddSystemEvent(new PersonPersonalInformationChangedFailed(new string[] { "Not found." }, command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); //consider calling something else like 'ProcessEvents' as no saving is happening, but keeping it consistance is nice too and permits lesser likelihood of programmer error
            return; 
        }

        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        BinaryFlag flag = new PersonChangePersonalInformationValidator(command, validationData).Validate();
        if (!flag)
        { 
            _unitOfWork.AddSystemEvent(new PersonPersonalInformationChangedFailed(PersonErrorConversion.Convert(flag), command.CorrelationId, command.CommandId));
            _unitOfWork.Save(); 
            return; 
        }

        if (command.FirstName is not null)
        {
            entity.ReplaceFirstName(command.FirstName.FirstName);
            entity.AddDomainEvent(new PersonChangedFirstName(entity, command.CorrelationId, command.CausationId));
        }
        if (command.LastName is not null)
        {
            entity.ReplaceLastName(command.LastName.LastName);
            entity.AddDomainEvent(new PersonChangedLastName(entity, command.CorrelationId, command.CausationId));
        }
        if (command.Birth is not null)
        { //integration event
            entity.UpdateBirth(command.Birth.Birth);
            entity.AddDomainEvent(new PersonChangedBirth(entity, command.CorrelationId, command.CausationId));
        }
        if (command.Gender is not null && entity.Gender != command.Gender.Gender)
        {
            var oldGender = entity.Gender;
            entity.UpdateGenderIdentification(command.Gender.Gender);
            _unitOfWork.AddSystemEvent(new PersonReplacedGender(entity, oldGender, command.CorrelationId, command.CommandId));
            entity.AddDomainEvent(new PersonChangedGender(entity, command.CorrelationId, command.CommandId));
        }

        var firstNameChanged = command.FirstName is not null;
        var lastNameChanged = command.LastName is not null;
        var birthChanged = command.Birth is not null;
        var genderChanged = command.Gender is not null && entity.Gender != command.Gender.Gender;
        _unitOfWork.AddSystemEvent(new PersonPersonalInformationChangedSuccessed(firstNameChanged, lastNameChanged, birthChanged, genderChanged, command.CorrelationId, command.CommandId));
        _unitOfWork.PersonRepository.UpdatePersonalInformation(entity);
        _unitOfWork.Save();
        return;
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
            return; ;
        }
        result.Data.AddDomainEvent(new GenderRecognisedSucceeded(result.Data, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Recognise(result.Data);
        _unitOfWork.Save();
        return; 
    }

    public void Handle(AddPersonToGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result; 
        if (entity is null)
        {
            _unitOfWork.AddSystemEvent(new PersonAddedToGenderFailed(command.PersonId, command.GenderId, new string[] { $"Gender {command.GenderId} was not found." }, command.CorrelationId, command.CommandId)); ;
            _unitOfWork.Save();
            return; //create an event for a saga to handle and stop the execution of the current code.
        } //should validate if the person id exist
        entity.AddPerson(command.PersonId);
        entity.AddDomainEvent(new PersonAddedToGenderSucceeded(entity, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entity);
        return; 
    }

    public void Handle(RemovePersonFromGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result;
        if (entity is null)
        {
            return; //create an event for a saga to handle and stop the execution of the current code.
            //cause an error that will prevent the saving of person.
        } //these handlers should, in theory, not fail as they rely on validated data.
        entity.RemovePerson(command.PersonId); //no need for validating if the person exist or not as they are getting removed.
        entity.AddDomainEvent(new PersonRemovedFromGenderSucceeded(entity, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entity);
        return; 
    }

    public void Handle(ChangePersonGender command)
    {
        var entityToAddToo = _unitOfWork.GenderRepository.GetForOperationAsync(command.NewGenderId).Result;
        var entityToRemoveFrom = _unitOfWork.GenderRepository.GetForOperationAsync(command.OldGenderId).Result;
        if (entityToAddToo is null || entityToRemoveFrom is null)
        {
            return; 
        }
        entityToAddToo.AddPerson(command.PersonId);
        entityToAddToo.AddDomainEvent(new PersonAddedToGenderSucceeded(entityToAddToo, command.PersonId, command.CorrelationId, command.CommandId));
        entityToRemoveFrom.RemovePerson(command.PersonId);
        entityToRemoveFrom.AddDomainEvent(new PersonRemovedFromGenderSucceeded(entityToRemoveFrom, command.PersonId, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Update(entityToAddToo);
        _unitOfWork.GenderRepository.Update(entityToRemoveFrom);
        return; 
    }

    public void Handle(UnrecogniseGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            _unitOfWork.AddSystemEvent(new GenderUnrecognisedFailed(new string[] {"Not Found."}, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return; 
        }
        if (entity.People.Any())
        {
            _unitOfWork.AddSystemEvent(new GenderUnrecognisedFailed(new string[] { $"People identify with gender {entity.VerbSubject}/{entity.VerbObject}." }, command.CorrelationId, command.CommandId));
            _unitOfWork.Save();
            return;
        }
        entity.AddDomainEvent(new GenderUnrecognisedSucceeded(entity, command.CorrelationId, command.CommandId));
        _unitOfWork.GenderRepository.Unrecognise(entity);
        _unitOfWork.Save();
        return; 
    }
}
