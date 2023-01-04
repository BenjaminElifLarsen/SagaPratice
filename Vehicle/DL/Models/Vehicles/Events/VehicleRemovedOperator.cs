using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleRemovedOperator : DomainEvent
{
    public int OperatorId { get; private set; }

    internal VehicleRemovedOperator(Vehicle aggregate, int operatorId, Guid correlationId, Guid causationId)
        : base(aggregate.Id, aggregate.GetType().Name, aggregate.Events.Count(), correlationId, causationId)
    {
        OperatorId = operatorId;
    }
}