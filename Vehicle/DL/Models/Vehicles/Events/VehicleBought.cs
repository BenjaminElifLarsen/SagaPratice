using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleBought : DomainEvent
{
    internal VehicleBought(Vehicle aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
    }
}
