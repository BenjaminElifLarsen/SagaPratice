using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
public sealed record FiredSucceeded : StateEvent
{
    public FiredSucceeded(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}
