using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleStartedFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }
    public int VehicleId { get; private set; }

    public VehicleStartedFailed(int vehicleId, IEnumerable<string> errors, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        VehicleId = vehicleId;
        Errors = errors;
    }
}
