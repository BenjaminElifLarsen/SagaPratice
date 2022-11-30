using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorLicenseExpired : IDomainEvent<OperatorLicenseExpiredData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorLicenseExpiredData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public OperatorLicenseExpired(Operator aggregate, int licenseTypeId, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(aggregate.OperatorId, licenseTypeId);
    }
}

public class OperatorLicenseExpiredData
{
    public int OperatorId { get; private set; }
    public int LicenseTypeId { get; private set; }

    public OperatorLicenseExpiredData(int operatorId, int licenseTypeId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}