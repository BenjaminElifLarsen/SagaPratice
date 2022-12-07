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

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal OperatorLicenseRetracted(Operator aggregate, License license, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(aggregate.OperatorId, license.Type.Id, aggregate.Vehicles.Select(x => x.Id));
    }
}

public class OperatorLicenseRetractedData
{ //consider moving these into the class above them. Tried and not to happy with the design
    public int OperatorId { get; private set; }
    public int LicenseTypeId { get; private set; } //license type id of the retracted license
    public IEnumerable<int> VehicleIds { get; private set; }

    internal OperatorLicenseRetractedData(int operatorId, int licenseTypeId, IEnumerable<int> vehicleIds)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
        VehicleIds = vehicleIds;
    }
}