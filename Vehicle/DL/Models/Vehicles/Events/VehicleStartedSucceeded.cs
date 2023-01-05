using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleStartedSucceeded : DomainEvent
{
    internal VehicleStartedSucceeded(Vehicle aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
    }
}
