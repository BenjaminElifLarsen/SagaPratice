using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class PermittedToOperate : IDomainEvent<PermittedToStartData>
{
    public PermittedToStartData Data { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal PermittedToOperate()
    {

    }
}

public class PermittedToStartData
{
    //public int OperatorId { get; set; }
    public int VehicleId { get; set; }
}
