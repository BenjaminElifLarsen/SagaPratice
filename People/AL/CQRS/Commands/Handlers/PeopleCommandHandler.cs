using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.CQRS.Queries;
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
    private readonly IPersonEventPublisher _personEventPublisher;
    private readonly IUnitOfWork _unitOfWork;

    public PeopleCommandHandler(IPersonFactory personFactory, IGenderFactory genderFactory, IPersonEventPublisher personEventPublisher, IUnitOfWork unitOfWork)
    {
        _personFactory = personFactory;
        _genderFactory = genderFactory;
        _personEventPublisher = personEventPublisher;
        _unitOfWork = unitOfWork;
    }

    public Result Handle(HirePersonFromUser command)
    { //trigger event. Gender will also need to know of the new person
        var genderIds = _unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result;
        var validationData = new PersonValidationData(genderIds);
        var result = _personFactory.CreatePerson(command, validationData);
        if (result is InvalidResult<Person>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.PersonRepository.Hire(result.Data);
        try
        { //this method knows what would be observing the event, which does not make sense.
            _personEventPublisher.RegisterHandler<PersonHired>(AddPersonToGender);
            result.Data.AddDomainEvent(new PersonHired(result.Data));
           // _personEventPublisher.Hire(result.Data);
            //result.Data.AddDomainEvent(new PersonHired(result.Data));
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        finally
        {
            _personEventPublisher.UnregisterHandler<PersonHired>(AddPersonToGender); //if given PersonFired no error occur, look into how to solve this.
        }
        return new SuccessResultNoData();
    } //maybe move all of the handler implementation code into domain services, one for each aggregate roots

    public Result Handle(FirePersonFromUser command)
    { //trigger event
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.FiredFrom.Year, command.FiredFrom.Month, command.FiredFrom.Day));
            _unitOfWork.PersonRepository.Fire(entity);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(ChangePersonalInformationFromUser command)
    { //trigger event only if birth was changed
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData("");
        }

        throw new NotImplementedException();
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

    private void AddPersonToGender(PersonHired e)
    {
        Handle(new AddPersonToGender(e.Data.PersonId, e.Data.GenderId));
    }

    private Result Handle(AddPersonToGender command)
    { //cannot send result back this way, this code should not fail, but not to happy about this design as it currently stand, here, AddPersonToGender, and Handle(HirePersonFromUser command). 
        //consider having adding a unit of work for saving in all repository in the bounded context at ones. Would require rewritting how the mock context work (then again it is only for testing, so if it is not fullly correct is fine enough).
        //there should not be any erros anyway given the simplicity of the domain models and validation.
        //maybe move the event creation and raising over to the ipl save as plenty of sources states that the save() could trigger any events before actually saving the data. This save would be the one in the unit of work.
        var gender = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result;
        if(gender is null)
        {
            //cause an error that will prevent the saving of person. Could modify the event args to have a collection of responses
        }
        gender.AddPerson(new(command.PersonId));
        _unitOfWork.GenderRepository.Update(gender);
        return new SuccessResultNoData();
    }
}
