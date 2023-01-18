using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
using PersonDomain.DL.Events.Domain;
using PersonDomain.IPL.Repositories.ProcesserManagers;

namespace PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
internal class GenderUnrecogniseProcessRouter : IGenderUnrecogniseProcessRouter
{
    private readonly IGenderUnrecogniseProcessRepository _repository;
    private readonly IPersonCommandBus _commandBus;
    private readonly IPersonDomainEventBus _eventBus;

    public GenderUnrecogniseProcessRouter(IGenderUnrecogniseProcessRepository repository, IPersonCommandBus commandBus, IPersonDomainEventBus eventBus)
    {
        _repository = repository;
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public void Handle(GenderUnrecognisedSucceeded @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(GenderUnrecognisedFailed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    private void Transmit(GenderUnrecogniseProcessManager pm)
    {
        foreach (var c in pm.Commands)
        {
            _commandBus.Dispatch(c);
        }
        foreach (var e in pm.StateEvents)
        {
            _eventBus.Publish(e);
        }
    }
}
