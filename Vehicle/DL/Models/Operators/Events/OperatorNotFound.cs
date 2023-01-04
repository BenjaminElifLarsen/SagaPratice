using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorNotFound : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    internal OperatorNotFound(int id, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Operator).Name;
        AggregateId = id;
        Errors = errors;
    }
}