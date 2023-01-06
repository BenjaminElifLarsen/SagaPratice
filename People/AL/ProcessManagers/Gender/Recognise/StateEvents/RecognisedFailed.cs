using Common.Events.State;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
public sealed record RecognisedFailed : StateEvent
{
    public string VerbSubject { get; private set; }
    public string VerbObject { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public RecognisedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }
}
