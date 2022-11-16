using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorLicenseRetracted : IDomainEvent<OperatorLicenseRetractedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorLicenseRetractedData Data { get; private set; }

    internal OperatorLicenseRetracted(Operator aggregate, License license)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.OperatorId, license.Type.Id);
    }
}

public class OperatorLicenseRetractedData
{ //consider moving these into the class above them. Tried and not to happy with the design
    public int PersonId { get; private set; }
    public int LicenseTypeId { get; private set; } //license type id of the retracted license

    internal OperatorLicenseRetractedData(int personId, int licenseTypeId)
    {
        PersonId = personId;
        LicenseTypeId = licenseTypeId;
    }
}