using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
internal class LicenseTypeAgeRequirementValidated : IDomainEvent<LicenseTypeAgeRequirementValidatedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public LicenseTypeAgeRequirementValidatedData Data { get; private set; }

    public LicenseTypeAgeRequirementValidated(int aggregateId, IEnumerable<int> operatorIdsNotFound, IEnumerable<int> operatorIdsNotValid, Guid correlationId, Guid causationId)
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

internal class LicenseTypeAgeRequirementValidatedData
{
    public IEnumerable<int> OperatorIdsNotFound { get; private set; }
    public IEnumerable<int> OperatorIdsNotValid { get; private set; }

    public LicenseTypeAgeRequirementValidatedData(IEnumerable<int> operatorIdsNotFound, IEnumerable<int> operatorIdsNotValid)
    {
        OperatorIdsNotFound = operatorIdsNotFound;
        OperatorIdsNotValid = operatorIdsNotValid;
    }
}
