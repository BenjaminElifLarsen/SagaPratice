using Common.Events.Domain;
using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.DL.Events.Domain;
public class DomainEventRegistry : BaseDomainEventRegistry //make either a contract with registrate and unregistrate or an abstract class with those (and a ctor)
{ //make this into a proper class that can be di into the domain command handler, so the command handler does not take domain event bus anymore.
  //unit of work takes in an instance of the class and calls event through it instead of directly through the bus and can, after saving, call the unregistrate. Perhaps.
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IDomainEventBus _domainEventBus;

    public DomainEventRegistry(IUnitOfWork unitOfWork, IDomainEventBus domainEventBus) : base(domainEventBus)
    {
        _unitOfWork = unitOfWork;
    } //got ciruclar dependency with unit of work, how to best to solve it? Method injection of Unit of Work? Move them away from each other? Make this a variable of MockDomainEventBus? 

    //private void test<T>(Action<T> t) where T : IDomainEvent
    //{ //what makes most sense, have a big registrate like RegistrateEventHandlers or something like this method?
    //    _domainEventBus.RegisterHandler<T>(t);
    //}

    public override void RegistrateEventHandlers()
    {
        _domainEventBus.RegisterHandler<PersonHired>(AddPersonToGender);
        _domainEventBus.RegisterHandler<PersonFired>(RemovePersomFromGender);
    }

    public override void UnregistrateEventHandlers()
    {
        _domainEventBus.UnregisterHandler<PersonHired>(AddPersonToGender);
        _domainEventBus.UnregisterHandler<PersonFired>(RemovePersomFromGender);
    }

    private void AddPersonToGender(PersonHired e)
    {
        Handle(new AddPersonToGender(e.Data.PersonId, e.Data.GenderId));
    }

    private Result Handle(AddPersonToGender command)
    { //cannot send result back this way, this code should not fail, but not to happy about this design as it currently stand, here, AddPersonToGender, and Handle(HirePersonFromUser command). 
      //consider having adding a unit of work for saving in all repository in the bounded context at ones. Would require rewritting how the mock context work (then again it is only for testing, so if it is not fullly correct is fine enough).
      //there should not be any erros anyway given the simplicity of the domain models and validation.
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result;
        if (entity is null)
        {
            throw new Exception();//cause an error that will prevent the saving of person.
        }
        entity.AddPerson(new(command.PersonId));
        _unitOfWork.GenderRepository.Update(entity);
        return new SuccessResultNoData();
    }

    private void RemovePersomFromGender(PersonFired e)
    {
        Handle(new RemovePersonFromGender(e.Data.PersonId, e.Data.GenderId));
    }

    private Result Handle(RemovePersonFromGender command)
    {
        var entity = _unitOfWork.GenderRepository.GetForOperationAsync(command.GenderId).Result;
        if (entity is null)
        {
            throw new Exception();
            //cause an error that will prevent the saving of person.
        } //these handlers should, in theory, not fail as they rely on validated data.
        entity.RemovePerson(new(command.PersonId));
        _unitOfWork.GenderRepository.Update(entity);
        return new SuccessResultNoData();
    }
}
