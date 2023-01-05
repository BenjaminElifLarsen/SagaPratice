using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorNotFound : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public Guid AggregateId { get; private set; }

    internal OperatorNotFound(Guid id, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateId = id;
        Errors = errors;
    }
}