using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class VehicleOperatorRelationshipEstablished : IDomainEvent<VehicleOperatorRelationshipEstablishedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public VehicleOperatorRelationshipEstablishedData Data { get; private set; }

    internal VehicleOperatorRelationshipEstablished(Vehicle aggregate, int operatorId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.VehicleId, operatorId);
    }
}

public class VehicleOperatorRelationshipEstablishedData
{
    public int OperatorId { get; private set; }
    public int VehicleId { get; private set; }

    internal VehicleOperatorRelationshipEstablishedData(int vehicleId, int operatorId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
    }
}