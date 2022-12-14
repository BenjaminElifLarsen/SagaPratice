using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Person.Hire;
internal sealed class HireProcessManager : IHireProcessManager
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public bool Running => !_trackerCollection.AllFinishedOrFailed;

    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public HireProcessManager(IPeopleCommandBus commandBus)
    {
        ProcessManagerId = Guid.NewGuid();
        _commandBus = commandBus;
        _handlers = new();
        _errors = new();
        _trackerCollection = new();
        _trackerCollection.AddEventTracker<PersonHiredSucceeded>(true);
        _trackerCollection.AddEventTracker<PersonHiredFailed>(false);
    }

    public void Handler(PersonHiredSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonHiredSucceeded>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<PersonHiredFailed>();

        _trackerCollection.AddEventTracker<PersonAddedToGenderSucceeded>(true);
        _trackerCollection.AddEventTracker<PersonAddedToGenderFailed>(false);

        _commandBus.Dispatch(new AddPersonToGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));

        PublishEventIfPossible();
    }

    public void Handler(PersonHiredFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonHiredSucceeded>(DomainEventStatus.Failed);
        _trackerCollection.UpdateEvent<PersonHiredFailed>(DomainEventStatus.Completed);

        _errors.AddRange(@event.Errors);
        PublishEventIfPossible();
    }

    public void Handler(PersonAddedToGenderSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonAddedToGenderSucceeded>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<PersonAddedToGenderFailed>();

        PublishEventIfPossible();
    }

    public void Handler(PersonAddedToGenderFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<PersonAddedToGenderSucceeded>(DomainEventStatus.Failed);
        _trackerCollection.UpdateEvent<PersonAddedToGenderFailed>(DomainEventStatus.Completed);

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
        CorrelationId = correlationId;
    }
}
