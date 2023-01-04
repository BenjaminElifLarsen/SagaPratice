using Common.Events.Domain;
using Common.Events.System;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record GenderUnrecognisedFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public GenderUnrecognisedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;

    }
}
