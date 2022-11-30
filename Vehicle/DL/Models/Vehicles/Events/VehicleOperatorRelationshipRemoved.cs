using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class VehicleOperatorRelationshipDisbanded : IDomainEvent<VehicleOperatorRelationshipDisbandedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public VehicleOperatorRelationshipDisbandedData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public VehicleOperatorRelationshipDisbanded(Vehicle aggregate, int operatorId, int version, Guid correlationId, Guid causationId)
    { //could have a ctor(Operator, int) version
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(aggregate.VehicleId, operatorId);
    }
}

public class VehicleOperatorRelationshipDisbandedData
{
    public int VehicleId { get; private set; }
    public int OperatorId { get; private set; }

    public VehicleOperatorRelationshipDisbandedData(int vehicleId, int operatorId)
    {
        VehicleId = vehicleId;
        OperatorId = operatorId;
    }
}