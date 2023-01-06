using Common.Events.System;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonHiredFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }


    internal PersonHiredFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    { 
        Errors = errors;
    }
}
