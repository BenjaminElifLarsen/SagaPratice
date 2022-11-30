using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorRemoved : IDomainEvent<OperatorRemovedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorRemovedData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public OperatorRemoved(Operator aggregate, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(aggregate.OperatorId, aggregate.Vehicles.Select(x => x.Id), aggregate.Licenses.Select(x => x.Type.Id));
    }
}

public class OperatorRemovedData
{
    public int Id { get; private set; } //maybe have a collection of vehicle ids and licesne type ids
    public IEnumerable<int> VehicleIds { get; private set; }
    public IEnumerable<int> LicenseTypeIds { get; private set; }

    internal OperatorRemovedData(int id, IEnumerable<int> vehicleIds, IEnumerable<int> licenseTypeIds)
    {
        Id = id;
        VehicleIds = vehicleIds;
        LicenseTypeIds = licenseTypeIds;
    }
}
