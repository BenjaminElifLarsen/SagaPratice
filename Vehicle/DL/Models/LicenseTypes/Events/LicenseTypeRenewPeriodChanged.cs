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

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal LicenseTypeRenewPeriodChanged(LicenseType aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.LicenseTypeId, aggregate.AgeRequirementInYears, aggregate.Operators.Select(x => x.Id));
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}

public class LicenseTypeRenewPeriodChangedData
{
    public int Id { get; private set; }
    public byte NewRenewPeriodInYears { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    internal LicenseTypeRenewPeriodChangedData(int id, byte newRenewPeriodInYears, IEnumerable<int> operatorIds)
    {
        Id = id;
        NewRenewPeriodInYears = newRenewPeriodInYears;
        OperatorIds = operatorIds;
    }
}