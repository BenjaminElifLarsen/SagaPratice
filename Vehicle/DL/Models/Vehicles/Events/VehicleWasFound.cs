using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleWasFound : SystemEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    internal VehicleWasFound(int vehicleId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = vehicleId;
    }
}
