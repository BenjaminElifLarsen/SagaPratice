using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleNotRequiredToRemoveOperator : SystemEvent
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }
    internal VehicleNotRequiredToRemoveOperator(Vehicle aggregate, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
    }
}
