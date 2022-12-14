using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Fire;
internal sealed class FireProcessManager : IFireProcessManager
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly EventStateCollection _trackerCollection;
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
    }

    public void SetUp(Guid correlationId)
    {
        if(CorrelationId == default)
        {
            CorrelationId = correlationId;
            _trackerCollection.AddEventTracker<PersonFiredSucceeded>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<PersonFiredFailed>(false, DomainEventType.Failer);
        }
    }

    public void Handler(PersonFiredSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.CompleteEvent<PersonFiredSucceeded>();
        _trackerCollection.RemoveEvent<PersonFiredFailed>();

        _trackerCollection.AddEventTracker<PersonRemovedFromGenderSucceeded>(true, DomainEventType.Succeeder); 
        _trackerCollection.AddEventTracker<PersonRemovedFromGenderFailed>(false, DomainEventType.Failer);

        _commandBus.Dispatch(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));

        PublishEventIfPossible();
    }

    public void Handler(PersonFiredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.FailEvent<PersonFiredSucceeded>();
        _trackerCollection.CompleteEvent<PersonFiredFailed>();

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(PersonRemovedFromGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.CompleteEvent<PersonRemovedFromGenderSucceeded>();
        _trackerCollection.RemoveEvent<PersonRemovedFromGenderFailed>();

        PublishEventIfPossible();
    }

    public void Handler(PersonRemovedFromGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.FailEvent<PersonRemovedFromGenderSucceeded>();
        _trackerCollection.CompleteEvent<PersonRemovedFromGenderFailed>();

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
