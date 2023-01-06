using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.Busses.Event;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.IPL.Repositories.GenderRecogniseProcessRepository;

namespace PeopleDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
internal class GenderRecogniseProcessRouter : IGenderRecogniseProcessRouter
{ //could have a middleware for this regarding setting up the routes and handlers between the event bus and this class
    private readonly IGenderRecogniseProcessRepository _repository;
    private readonly IPersonCommandBus _commandBus;
    private readonly IPersonDomainEventBus _eventBus;

    public GenderRecogniseProcessRouter(IGenderRecogniseProcessRepository repository, IPersonCommandBus commandBus, IPersonDomainEventBus eventBus)
    { //the work this is inspired off state the repo should send the commands, but I feel more like this class should do it, the repo should only care about storing and retrieving data from the context
        _repository = repository; //also other sources states commands should be send before saving (of course those did not save the pm [and in some cases did not have such])
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public async void Handle(GenderRecognisedSucceeded @event)
    {
        var pm = await Task.Run(() => _repository.LoadAsync(@event.CorrelationId));
        pm ??= new GenderRecogniseProcessManager(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        foreach(var c in pm.Commands)
        {
            _commandBus.Dispatch(c);
        }
        foreach(var e in pm.StateEvents)
        {
            _eventBus.Publish(e);
        }
    }

    public void Handle(GenderRecognisedFailed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new GenderRecogniseProcessManager(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
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
