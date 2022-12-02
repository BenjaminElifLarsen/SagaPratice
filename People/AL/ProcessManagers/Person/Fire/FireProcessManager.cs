using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Fire;
internal sealed class FireProcessManager : IFireProcessManager
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;
    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public FireProcessManager(IPeopleCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
        _trackerCollection.AddEventTracker<PersonFiredSuccessed>(true);
        _trackerCollection.AddEventTracker<PersonFiredFailed>(false);
    }

    public void SetUp(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public void Handler(PersonFiredSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonFiredSuccessed>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<PersonFiredFailed>();

        _trackerCollection.AddEventTracker<PersonRemovedFromGenderSuccessed>(true); 
        _trackerCollection.AddEventTracker<PersonRemovedFromGenderFailed>(false);

        _commandBus.Dispatch(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));

        PublishEventIfPossible();
    }

    public void Handler(PersonFiredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonFiredSuccessed>(DomainEventStatus.Failed);
        _trackerCollection.UpdateEvent<PersonFiredFailed>(DomainEventStatus.Completed);

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(PersonRemovedFromGenderSuccessed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSuccessed>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<PersonRemovedFromGenderFailed>();

        PublishEventIfPossible();
    }

    public void Handler(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonRemovedFromGenderSuccessed>(DomainEventStatus.Failed);
        _trackerCollection.UpdateEvent<PersonRemovedFromGenderFailed>(DomainEventStatus.Completed);

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void PublishEventIfPossible()
    {
        if (_trackerCollection.AllFinishedOrFailed)
        {
            Result result = !_trackerCollection.Failed ? new SuccessResultNoData() : new InvalidResultNoData(_errors.ToArray());
            ProcesserFinished @event = new(result, ProcessManagerId);
            foreach (var handler in _handlers)
            {
                handler.Invoke(@event);
            }
        }
    }

    public void RegistrateHandler(Action<ProcesserFinished> handler)
    {
        _handlers.Add(handler);
    }
}
