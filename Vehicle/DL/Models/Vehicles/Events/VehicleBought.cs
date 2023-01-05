using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleBought : DomainEvent
{
    internal VehicleBought(Vehicle aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
    }
}
