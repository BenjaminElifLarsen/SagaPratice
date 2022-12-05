using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
internal class LicenseTypeAlteredSuccesed : IDomainEvent<LicenseTypeAlteredSuccesedData>
{ //do similarly to the one over in People
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public LicenseTypeAlteredSuccesedData Data { get; private set; }

    public LicenseTypeAlteredSuccesed(LicenseType aggregate, int version, bool typeChanged, bool ageRequirementChanged, bool renewPeriodChanged, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(typeChanged, ageRequirementChanged, renewPeriodChanged);
    }
}

internal class LicenseTypeAlteredSuccesedData
{
    public bool TypeChanged { get; private set; }
    public bool AgeRequirementChanged { get; private set; }
    public bool RenewPeriodChanged { get; private set; }

    public LicenseTypeAlteredSuccesedData(bool typeChanged, bool ageRequirementChanged, bool renewPeriodChanged)
    {
        TypeChanged = typeChanged;
        AgeRequirementChanged = ageRequirementChanged;
        RenewPeriodChanged = renewPeriodChanged;
    }
}