using Common.Events.System;

namespace PeopleDomain.DL.Events.Domain;
public sealed record GenderRecognisedFailed : SystemEvent //system event, since it did not up altering any data and does not belong to any aggregate
{
    public IEnumerable<string> Errors { get; private set; }

    public GenderRecognisedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
    }
}
