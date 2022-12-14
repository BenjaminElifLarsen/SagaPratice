using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise;
internal sealed class RecogniseProcessManager : IRecogniseProcessManager
{
    private readonly IPeopleCommandBus _commandBus;
    private readonly EventStateCollection _trackerCollection;
    private readonly List<string> _errors;
    private readonly HashSet<Action<ProcesserFinished>> _handlers;

    public Guid ProcessManagerId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public bool Running => !_trackerCollection.AllFinishedOrFailed;
    public bool FinishedSuccessful => _trackerCollection.AllRequiredSucceded;

    public RecogniseProcessManager(IPeopleCommandBus commandBus)
    {
        _commandBus = commandBus;
        ProcessManagerId = Guid.NewGuid();
        _errors = new();
        _handlers = new();
        _trackerCollection = new();
    }

    public void Handler(GenderRecognisedSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;


        _trackerCollection.CompleteEvent<GenderRecognisedSucceeded>();
        _trackerCollection.RemoveEvent<GenderRecognisedFailed>();

        PublishEventIfPossible();
    }

    public void Handler(GenderRecognisedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        _trackerCollection.FailEvent<GenderRecognisedSucceeded>();
        _trackerCollection.CompleteEvent<GenderRecognisedFailed>();

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
            _trackerCollection.AddEventTracker<GenderRecognisedSucceeded>(true, DomainEventType.Succeeder);
            _trackerCollection.AddEventTracker<GenderRecognisedFailed>(false, DomainEventType.Failer);
        }
    }
}
