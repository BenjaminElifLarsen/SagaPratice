
using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
public sealed record RemovedFromGenderSucceeded : StateEvent
{
    public RemovedFromGenderSucceeded(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}
