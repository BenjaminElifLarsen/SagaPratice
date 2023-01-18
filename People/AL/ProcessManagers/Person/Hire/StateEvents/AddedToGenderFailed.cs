using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
public sealed record AddedToGenderFailed : StateEvent
{
    public IEnumerable<string> Errors { get; private set; }
    
    public AddedToGenderFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }
}
