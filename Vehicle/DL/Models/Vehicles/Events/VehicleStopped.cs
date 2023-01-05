using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleStopped : DomainEvent
{
    internal VehicleStopped(Vehicle aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
    
    }
}
