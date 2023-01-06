using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Hire;
internal sealed class HireProcessManager : IHireProcessManager
{
    private readonly IPersonCommandBus _commandBus;
    private readonly EventStateCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public bool Running => !_trackerCollection.AllFinishedOrFailed;

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public HireProcessManager(IPersonCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
    }

    public void Handle(PersonHiredSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.CompleteEvent<PersonHiredSucceeded>();
        _trackerCollection.RemoveEvent<PersonHiredFailed>();

        _trackerCollection.AddEventTracker<PersonAddedToGenderSucceeded>(true, DomainEventType.Succeeder);
        _trackerCollection.AddEventTracker<PersonAddedToGenderFailed>(false, DomainEventType.Failer);

        _commandBus.Dispatch(new AddPersonToGender(@event.AggregateId, @event.GenderId, @event.CorrelationId, @event.EventId));

        PublishEventIfPossible();
    }

    public void Handle(PersonHiredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.FailEvent<PersonHiredSucceeded>();
        _trackerCollection.CompleteEvent<PersonHiredFailed>();

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handle(PersonAddedToGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.CompleteEvent<PersonAddedToGenderSucceeded>();
        _trackerCollection.RemoveEvent<PersonAddedToGenderFailed>();

        PublishEventIfPossible();
    }

    public void Handle(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.FailEvent<PersonAddedToGenderSucceeded>();
        _trackerCollection.CompleteEvent<PersonAddedToGenderFailed>();

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

    public void SetUp(Guid correlationId)
    {
        if(CorrelationId == default)
        {
            CorrelationId = correlationId;
            _trackerCollection.AddEventTracker<PersonHiredSucceeded>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<PersonHiredFailed>(false, DomainEventType.Failer);
        }
    }
}
