using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
public sealed record HiredSucceeded : StateEvent
{
    public HiredSucceeded(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}
