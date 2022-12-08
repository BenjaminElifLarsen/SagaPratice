using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class VehiclesFoundWithSpecificVehicleInformationAndOperator : IDomainEvent<VehiclesFoundWithSpecificVehicleInformationAndOperatorData>
{
    public VehiclesFoundWithSpecificVehicleInformationAndOperatorData Data { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal VehiclesFoundWithSpecificVehicleInformationAndOperator(int operatorId, IEnumerable<int> vehicleIds, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = 0;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
        Data = new(operatorId, vehicleIds);
    }
}

public class VehiclesFoundWithSpecificVehicleInformationAndOperatorData
{
    public int OperatorId { get; private set; }
    public IEnumerable<int> VehicleIds { get; private set; }

    internal VehiclesFoundWithSpecificVehicleInformationAndOperatorData(int operatorId, IEnumerable<int> vehicleIds)
    {
        OperatorId = operatorId;
        VehicleIds = vehicleIds;
    }
}