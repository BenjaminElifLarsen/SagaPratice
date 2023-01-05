using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record AttemptToStartVehicleStarted : SystemEvent
{

    public Guid OperatorId { get; private set; }
    
    public Guid VehicleId { get; private set; }

    public AttemptToStartVehicleStarted(Guid vehicleId, Guid operatorId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
    }
}
