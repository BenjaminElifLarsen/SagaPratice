using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class AttemptToStartVehicleStarted : IDomainEvent<AttemptToStartVehicleStartedData>
{
    public string AggregateType { get; private set;}

    public int AggregateId { get; private set;}

    public string EventType { get; private set;}

    public Guid EventId { get; private set;}

    public long TimeStampRecorded { get; private set;}

    public Guid CorrelationId { get; private set;}

    public Guid CausationId { get; private set;}

    public int Version { get; private set;}

    public AttemptToStartVehicleStartedData Data { get; private set;}

    public AttemptToStartVehicleStarted(int vehicleId, int operatorId, Guid correlationId, Guid cuasationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = vehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = cuasationId;
        Version = 0;
        Data = new(operatorId, vehicleId);
    }
}

public class AttemptToStartVehicleStartedData
{
    public int OperatorId { get; private set; }
    public int VehicleId { get; private set; }

    internal AttemptToStartVehicleStartedData(int operatorId, int vehicleId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
    }
}