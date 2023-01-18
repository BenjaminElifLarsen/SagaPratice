using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.ProcessManagers.Person.Hire;
using PersonDomain.DL.Events.Domain;
using PersonDomain.IPL.Repositories.ProcesserManagers.People;

namespace PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
internal class PersonHireProcessRouter : IPersonHireProcessRouter
{
    public readonly IPersonHireProcessRepository _repository;
    public readonly IPersonCommandBus _commandBus;
    public readonly IPersonDomainEventBus _eventBus;

    public PersonHireProcessRouter(IPersonHireProcessRepository repository, IPersonCommandBus commandBus, IPersonDomainEventBus eventBus)
    {
        _repository = repository;
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public void Handle(PersonHiredSucceeded @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonHiredFailed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonAddedToGenderSucceeded @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonAddedToGenderFailed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId); //the assignment of a new object should never happen in anything but the start event, consider removing these and throw if pm is null
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    private void Transmit(HireProcessManager pm)
    {
        foreach (var c in pm.Commands)
        {
            _commandBus.Dispatch(c);
        }
        foreach (var e in pm.StateEvents)
        {
            _eventBus.Publish(e);
        }
        pm.RemoveAllCommands();
        pm.RemoveAllEvents();
    }
}
