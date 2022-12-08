using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public class LicenseTypeOperatorRemoved : IDomainEvent
{ //need a command handler and command for removing multiple operator ids from a license type
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public LicenseTypeOperatorRemoved(LicenseType aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.LicenseTypeId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = aggregate.Events.Count();

    }
}
