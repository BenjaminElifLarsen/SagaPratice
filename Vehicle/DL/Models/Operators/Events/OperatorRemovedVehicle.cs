using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorRemovedVehicle : DomainEvent
{
    public Guid VehicleId { get; private set; } 

    internal OperatorRemovedVehicle(Operator aggregate, Guid vehicleId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        VehicleId = vehicleId;
    }
}
