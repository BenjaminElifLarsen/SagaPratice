using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvents;
public sealed record GenderReplacedSucceeded : StateEvent
{
    public GenderReplacedSucceeded(Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
    }
}
