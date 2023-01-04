using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorWasFound : SystemEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public OperatorWasFound(int operatorId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Operator).Name;
        AggregateId = operatorId;
    }
}
