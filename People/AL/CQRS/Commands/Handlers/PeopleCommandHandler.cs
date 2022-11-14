using Common.Events.Domain;
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
    //private readonly IDomainEventBus _personEventPublisher;
    private readonly IUnitOfWork _unitOfWork;

    public PeopleCommandHandler(IPersonFactory personFactory, IGenderFactory genderFactory, IDomainEventBus domainEventBus, IUnitOfWork unitOfWork)
    {
        _personFactory = personFactory;
        _genderFactory = genderFactory;
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
            //_personEventPublisher.RegisterHandler<PersonHired>(AddPersonToGender);
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
            //_personEventPublisher.UnregisterHandler<PersonHired>(AddPersonToGender); 
        }
        return new SuccessResultNoData();
    } //maybe move all of the handler implementation code into domain services, one for each aggregate roots

    public Result Handle(FirePersonFromUser command)
    { //trigger event
        var entity = _unitOfWork.PersonRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(new(command.FiredFrom.Year, command.FiredFrom.Month, command.FiredFrom.Day));
            //_personEventPublisher.RegisterHandler<PersonFired>(RemovePersomFromGender); //figure out a better way to (un)registrate handlers
            entity.AddDomainEvent(new PersonFired(entity)); //the event should first be triggered when DeletedFrom is true
            _unitOfWork.PersonRepository.Fire(entity);
            _unitOfWork.Save();
            //_personEventPublisher.UnregisterHandler<PersonFired>(RemovePersomFromGender);
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
}
