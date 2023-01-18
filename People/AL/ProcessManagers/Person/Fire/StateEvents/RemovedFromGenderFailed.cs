using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
public sealed record RemovedFromGenderFailed : StateEvent
{
    public RemovedFromGenderFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }

    public IEnumerable<string> Errors { get; private set; }
}
