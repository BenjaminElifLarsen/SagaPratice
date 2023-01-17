using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleOperatorRelationshipDisbanded : DomainEvent
{
    public Guid OperatorId { get; private set; }
    
    public VehicleOperatorRelationshipDisbanded(Vehicle aggregate, Guid operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    { //could have a ctor(Operator, int) version
        OperatorId = operatorId;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}