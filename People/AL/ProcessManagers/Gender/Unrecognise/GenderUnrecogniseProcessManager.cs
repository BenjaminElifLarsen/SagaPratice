using Common.UnitOfWork;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise.StateEvents;
using PersonDomain.DL.Events.Domain;
using static PersonDomain.AL.ProcessManagers.Gender.Unrecognise.GenderUnrecogniseProcessManager.UnrecogniseGenderState;

namespace PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
public sealed class GenderUnrecogniseProcessManager : BaseProcessManager, IGenderUnrecogniseProcessManager
{
    public UnrecogniseGenderState State { get; private set; }
    public GenderUnrecogniseProcessManager(Guid correlationId) : base(correlationId)
    {
        ProcessManagerId = Guid.NewGuid();
        State = NotStarted;
    }

    public void Handle(GenderUnrecognisedSucceeded @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch(State)
        {
            case NotStarted:
                State = GenderUnrecognised;
                AddStateEvent(new UnrecognisedSucceeded(CorrelationId, @event.EventId));
                AddCommand(new SaveProcessedWork(CorrelationId, @event.EventId));
                break;

            case GenderUnrecognised:
                break;

            case GenderFailedToUnrecognise: 
                break;

            default:
                break;
        }
    }

    public void Handle(GenderUnrecognisedFailed @event)
    {
        if (@event.CorrelationId != CorrelationId) return;

        switch (State)
        {
            case NotStarted:
                State = GenderFailedToUnrecognise;
                AddErrors(@event.Errors);
                AddStateEvent(new UnrecognisedFailed(Errors, CorrelationId, @event.EventId));
                AddCommand(new DiscardProcesssedWork(Errors, CorrelationId, @event.EventId));
                break;

            case GenderUnrecognised:
                break;

            case GenderFailedToUnrecognise:
                break;

            default: break;
        }
    }

    public enum UnrecogniseGenderState
    {
        NotStarted = 1,
        GenderUnrecognised = 2,
        GenderFailedToUnrecognise = 3,

        Unknown = 0,
    }


}
