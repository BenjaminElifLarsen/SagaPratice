using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
/// <summary>
/// The license type has been retracted and thus license using it are no longer valid.
/// </summary>
public class LicenseTypeRetracted : IDomainEvent<LicenseTypeRetractedData>
{ // License Type has a ISoftDeleteFrom contract, so this event should first be published when that date has been reached.
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public LicenseTypeRetractedData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal LicenseTypeRetracted(LicenseType aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.LicenseTypeId);
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}

public class LicenseTypeRetractedData
{
    public int Id { get; private set; }

    internal LicenseTypeRetractedData(int id)
    {
        Id = id;
    }
}