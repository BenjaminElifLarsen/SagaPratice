using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleRemovedOperator : DomainEvent
{
    public int OperatorId { get; private set; }

    internal VehicleRemovedOperator(Vehicle aggregate, int operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorId = operatorId;
    }
}