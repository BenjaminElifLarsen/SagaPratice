using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PersonDomain.DL.Events.Domain;
using PersonDomain.IPL.Repositories.ProcesserManagers.People;

namespace PersonDomain.AL.ProcessManagers.Routers.PersonChangeInformationProcessRouter;
internal class PersonChangeInformationProcessRouter : IPersonChangeInformationProcessRouter
{
    private readonly IPersonChangeProcessRepository _repository;
    private readonly IPersonCommandBus _commandBus;
    private readonly IPersonDomainEventBus _eventBus;

    public PersonChangeInformationProcessRouter(IPersonChangeProcessRepository repository, IPersonCommandBus commandBus, IPersonDomainEventBus eventBus)
    {
        _repository = repository;
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public void Handle(PersonPersonalInformationChangedSuccessed @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonPersonalInformationChangedFailed @event)
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

    public void Handle(PersonRemovedFromGenderSucceeded @event)
    {
        var pm = _repository.LoadAsync(@event.CorrelationId).Result;
        pm ??= new(@event.CorrelationId);
        pm.Handle(@event);
        _repository.Save(pm);
        Transmit(pm);
    }

    public void Handle(PersonReplacedGender @event)
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

    private void Transmit(PersonalInformationChangeProcessManager pm)
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
