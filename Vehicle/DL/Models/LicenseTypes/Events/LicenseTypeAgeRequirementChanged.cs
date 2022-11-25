using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public class LicenseTypeAgeRequirementChanged : IDomainEvent<LicenseTypeAgeRequirementChangedData>
{
    public LicenseTypeAgeRequirementChangedData Data { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    internal LicenseTypeAgeRequirementChanged(LicenseType aggegate)
    {
        AggregateType = aggegate.GetType().Name;
        AggregateId = aggegate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggegate.LicenseTypeId, aggegate.AgeRequirementInYears, aggegate.Operators.Select(x => x.Id));
    }
}

public class LicenseTypeAgeRequirementChangedData
{
    public int Id { get; private set; }
    public byte NewAgeRequirement { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    public LicenseTypeAgeRequirementChangedData(int id, byte newAgeRequirement, IEnumerable<int> operatorIds)
    {
        Id = id;
        NewAgeRequirement = newAgeRequirement;
        OperatorIds = operatorIds;
    }
}