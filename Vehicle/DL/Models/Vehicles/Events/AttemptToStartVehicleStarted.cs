using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class AttemptToStartVehicleStarted : IDomainEvent<AttemptToStartVehicleStartedData>
{
    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();

    public Guid CorrelationId => throw new NotImplementedException();

    public Guid CausationId => throw new NotImplementedException();

    public int Version => throw new NotImplementedException();

    public AttemptToStartVehicleStartedData Data => throw new NotImplementedException();
}

public class AttemptToStartVehicleStartedData
{
    public int OperatorId { get; private set; }
    public int VehicleId { get; private set; }

    internal AttemptToStartVehicleStartedData()
    {

    }
}