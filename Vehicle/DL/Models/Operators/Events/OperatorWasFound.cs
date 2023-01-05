using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorWasFound : SystemEvent
{
    public Guid Id { get; private set; }

    public OperatorWasFound(Guid id, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Id = id;
    }
}
