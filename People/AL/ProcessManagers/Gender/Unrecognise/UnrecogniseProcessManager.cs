using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Command;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
internal sealed class UnrecogniseProcessManager : IUnrecogniseProcessManager
{
    private readonly IPersonCommandBus _commandBus;
    private readonly EventStateCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;
    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public UnrecogniseProcessManager(IPersonCommandBus commandBus)
    {
        _commandBus = commandBus;
        ProcessManagerId = Guid.NewGuid();
        _errors = new();
        _handlers = new();
        _trackerCollection = new();
    }

    public void Handle(GenderUnrecognisedSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.CompleteEvent<GenderUnrecognisedSucceeded>();
        _trackerCollection.RemoveEvent<GenderUnrecognisedFailed>();

        PublishEventIfPossible();
    }

    public void Handle(GenderUnrecognisedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.FailEvent<GenderUnrecognisedSucceeded>();
        _trackerCollection.CompleteEvent<GenderUnrecognisedFailed>();

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
        if(CorrelationId != default) 
        {
            CorrelationId = correlationId;
            _trackerCollection.AddEventTracker<GenderUnrecognisedSucceeded>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<GenderUnrecognisedFailed>(false, DomainEventType.Failer);
        }
    }
}
