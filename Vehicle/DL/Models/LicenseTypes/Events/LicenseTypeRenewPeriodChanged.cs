using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public class LicenseTypeRenewPeriodChanged : IDomainEvent<LicenseTypeRenewPeriodChangedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public LicenseTypeRenewPeriodChangedData Data { get; private set; }

    internal LicenseTypeRenewPeriodChanged(LicenseType aggregate)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.LicenseTypeId, aggregate.AgeRequirementInYears);
    }
}

public class LicenseTypeRenewPeriodChangedData
{
    public int Id { get; private set; }
    public byte NewRenewPeriodInYears { get; private set; }

    internal LicenseTypeRenewPeriodChangedData(int id, byte newRenewPeriodInYears)
    {
        Id = id;
        NewRenewPeriodInYears = newRenewPeriodInYears;
    }
}