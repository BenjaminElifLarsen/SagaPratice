using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Gender.Unrecognise.StateEvents;
public sealed record UnrecognisedFailed : StateEvent
{
    public string VerbSubject { get; private set; }
    public string VerbObject { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public UnrecognisedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }
}
