using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleOperatorRelationshipEstablished : DomainEvent
{
    public Guid OperatorId { get; private set; }

    internal VehicleOperatorRelationshipEstablished(Vehicle aggregate, Guid operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorId = operatorId;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}