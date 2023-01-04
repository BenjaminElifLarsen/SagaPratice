using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorRemovedVehicle : DomainEvent
{
    public int VehicleId { get; private set; } 

    internal OperatorRemovedVehicle(Operator aggregate, int vehicleId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        VehicleId = vehicleId;
    }
}
