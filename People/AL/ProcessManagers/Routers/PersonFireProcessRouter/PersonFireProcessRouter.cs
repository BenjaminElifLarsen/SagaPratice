using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.ProcessManagers.Person.Fire;
using PersonDomain.DL.Events.Domain;
using PersonDomain.IPL.Repositories.ProcesserManagers;

namespace PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
internal class PersonFireProcessRouter : IPersonFireProcessRouter
{
    private readonly IPersonFireProcessRepository _repository;
    private readonly IPersonCommandBus _commandBus;
    private readonly IPersonDomainEventBus _eventBus;

    public PersonFireProcessRouter(IPersonFireProcessRepository repository, IPersonCommandBus commandBus, IPersonDomainEventBus eventBus)
    {
        _repository = repository;
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public void Handle(PersonFiredSucceeded @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonFiredFailed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonRemovedFromGenderSucceeded @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonRemovedFromGenderFailed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    private void Transmit(FireProcessManager pm)
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
