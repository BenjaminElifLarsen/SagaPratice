using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Gender.Unrecognise.StateEvents;
public sealed record UnrecognisedSucceeded : StateEvent
{
    public int GenderId { get; set; }

    public UnrecognisedSucceeded(/*int Id,*/ Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        //GenderId = Id;
    }
}
