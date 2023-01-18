using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
public sealed record HiredFailed : StateEvent
{
    public IEnumerable<string> Errors { get; private set; }
    public HiredFailed(IEnumerable<string> errros, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errros;
    }
}
