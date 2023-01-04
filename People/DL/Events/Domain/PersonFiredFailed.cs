using Common.Events.System;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonFiredFailed : SystemEvent
{
    public string AggregateType { get; private set; }
    public int AggregateId { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public PersonFiredFailed(Person aggregate, IEnumerable<string> errors, int version, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        Errors = errors;
    }

    public PersonFiredFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Person).Name;
        AggregateId = 0;
        Errors = errors;
    }
}
