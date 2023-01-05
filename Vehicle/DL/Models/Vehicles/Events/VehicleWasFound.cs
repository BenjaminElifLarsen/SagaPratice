using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleWasFound : SystemEvent
{
    public string AggregateType { get; private set; }

    public Guid AggregateId { get; private set; }

    internal VehicleWasFound(Guid vehicleId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = vehicleId;
    }
}
