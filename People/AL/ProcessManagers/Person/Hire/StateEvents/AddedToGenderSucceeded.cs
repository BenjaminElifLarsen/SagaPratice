using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
public sealed record AddedToGenderSucceeded : StateEvent
{
    public AddedToGenderSucceeded(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}
