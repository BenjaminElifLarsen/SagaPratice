using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class VehicleRemovedOperator : IDomainEvent<VehicleRemovedOperatorData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public VehicleRemovedOperatorData Data { get; private set; }

    internal VehicleRemovedOperator(Vehicle aggregate, int operatorId, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = aggregate.OldEventsDesign.Count();
        Data = new(operatorId, aggregate.VehicleId);
    }
}

public class VehicleRemovedOperatorData
{
    public int OperatorId { get; private set; }
    public int VehicleId { get; private set; }

    public VehicleRemovedOperatorData(int operatorId, int vehicleId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
    }
}