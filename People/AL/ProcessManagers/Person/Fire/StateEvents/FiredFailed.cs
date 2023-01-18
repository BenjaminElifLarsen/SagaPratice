using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
public sealed record FiredFailed : StateEvent
{
    public IEnumerable<string> Errors { get; private set; }
    
    public FiredFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }
}
