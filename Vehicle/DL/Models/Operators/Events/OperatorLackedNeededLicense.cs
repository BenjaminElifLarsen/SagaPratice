using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLackedNeededLicense : SystemEvent
{
    public OperatorLackedNeededLicense(Guid correlationId, Guid causationId) 
        : base(correlationId, causationId)
    {
    }
}
