using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleOperatorRelationshipEstablished : DomainEvent
{
    public Guid OperatorId { get; private set; }

    internal VehicleOperatorRelationshipEstablished(Vehicle aggregate, Guid operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorId = operatorId;
    }
}