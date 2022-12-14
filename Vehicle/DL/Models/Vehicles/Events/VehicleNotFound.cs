using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
internal class VehicleNotFound : IDomainEventFail
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public IEnumerable<string> Errors { get; private set; }

    public VehicleNotFound(IEnumerable<string> errors, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = 0;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
        Errors = errors;
    }
}
