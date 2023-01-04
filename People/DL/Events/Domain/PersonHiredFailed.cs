using Common.Events.System;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonHiredFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }


    internal PersonHiredFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    { 
        Errors = errors;
    }
}
