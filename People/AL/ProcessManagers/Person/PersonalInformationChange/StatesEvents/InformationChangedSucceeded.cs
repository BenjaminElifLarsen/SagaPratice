using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvents;
public sealed record InformationChangedSucceeded : StateEvent
{
    public bool GenderWasChanged { get; private set; }
    public InformationChangedSucceeded(bool genderChanged, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        GenderWasChanged = genderChanged;
    }
}
