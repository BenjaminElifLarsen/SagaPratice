using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorGainedLicense : IDomainEvent<OperatorGainedLicenseData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorGainedLicenseData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal OperatorGainedLicense(Operator aggregate, License license, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(aggregate.OperatorId, license.Type.Id);
    }
}

public class OperatorGainedLicenseData
{
    public int OperatorId { get; set; }
    public int LicenseTypeId { get; set; }

    internal OperatorGainedLicenseData(int operatorId, int licenseTypeId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}