using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvents;
public sealed record GenderReplacedFailed : StateEvent
{
    public IEnumerable<string> Errors { get; private set; }
    public GenderReplacedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }
}
