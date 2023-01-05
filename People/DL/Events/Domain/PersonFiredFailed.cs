using Common.Events.System;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonFiredFailed : SystemEvent
{
    public string AggregateType { get; private set; }
    public Guid AggregateId { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public PersonFiredFailed(Person aggregate, IEnumerable<string> errors, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.Id;
        Errors = errors;
    }

    public PersonFiredFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        AggregateType = typeof(Person).Name;
        AggregateId = Guid.Empty;
        Errors = errors;
    }
}
