using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
internal class LicenseTypeAlteredFailed : IDomainEventFail
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public LicenseTypeAlteredFailed(LicenseType aggregate, IEnumerable<string> errors, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Errors = errors;
    }

    public LicenseTypeAlteredFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(LicenseType).Name;
        AggregateId = 0;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
        Errors = errors;
    }
}
