using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record AttemptToStartVehicleStarted : SystemEvent
{
    public string AggregateType { get; private set;}

    public int AggregateId { get; private set;}

    public int OperatorId { get; private set; }
    
    public int VehicleId { get; private set; }

    public AttemptToStartVehicleStarted(int vehicleId, int operatorId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = vehicleId;
        OperatorId = operatorId;
        VehicleId = vehicleId;
    }
}
