using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
[Obsolete("Might be removed in a future version", true)]
internal class LicenseTypeRenewPeriodValidated : IDomainEvent<LicenseTypeRenewPeriodValidatedData>
{ //not needed if not validating multiple aggregates via a single command
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public LicenseTypeRenewPeriodValidatedData Data { get; private set; }

    public LicenseTypeRenewPeriodValidated(int aggregateId, IEnumerable<int> operatorIdsNotFound, IEnumerable<int> operatorIdsNotValid, Guid correlationId, Guid causationId)
    {
        AggregateId = aggregateId;
        AggregateType = typeof(LicenseType).Name;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
        Data = new(operatorIdsNotFound, operatorIdsNotValid);
    }
}

internal class LicenseTypeRenewPeriodValidatedData
{
    public IEnumerable<int> OperatorIdsNotFound { get; private set; }
    public IEnumerable<int> OperatorIdsNotValid { get; private set; }

    public LicenseTypeRenewPeriodValidatedData(IEnumerable<int> operatorIdsNotFound, IEnumerable<int> operatorIdsNotValid)
    {
        OperatorIdsNotFound = operatorIdsNotFound;
        OperatorIdsNotValid = operatorIdsNotValid;
    }
}