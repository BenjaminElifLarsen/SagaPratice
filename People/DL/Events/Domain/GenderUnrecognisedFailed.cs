using Common.Events.Domain;
using Common.Events.System;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record GenderUnrecognisedFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public GenderUnrecognisedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;

    }
}
