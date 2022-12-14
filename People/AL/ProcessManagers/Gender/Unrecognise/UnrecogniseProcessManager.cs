using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Unrecognise;
internal sealed class UnrecogniseProcessManager : IUnrecogniseProcessManager
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly EventTrackerCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;
    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public UnrecogniseProcessManager(IPeopleCommandBus commandBus)
    {
        _commandBus = commandBus;
        ProcessManagerId = Guid.NewGuid();
        _errors = new();
        _handlers = new();
        _trackerCollection = new();
        _trackerCollection.AddEventTracker<GenderUnrecognisedSucceeded>(true);
        _trackerCollection.AddEventTracker<GenderUnrecognisedFailed>(false);
    }

    public void Handler(GenderUnrecognisedSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<GenderUnrecognisedSucceeded>(DomainEventStatus.Completed);
        _trackerCollection.RemoveEvent<GenderUnrecognisedFailed>();

        PublishEventIfPossible();
    }

    public void Handler(GenderUnrecognisedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.UpdateEvent<GenderUnrecognisedSucceeded>(DomainEventStatus.Failed);
        _trackerCollection.UpdateEvent<GenderUnrecognisedFailed>(DomainEventStatus.Completed);

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
