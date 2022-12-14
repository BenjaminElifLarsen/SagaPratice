using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class NotPermittedToOperate : IDomainEvent
{
    public string AggregateType { get; private set;}

    public int AggregateId { get; private set;}

    public string EventType { get; private set;}

    public Guid EventId { get; private set;}

    public long TimeStampRecorded { get; private set;}

    public Guid CorrelationId { get; private set;}

    public Guid CausationId { get; private set;}

    public int Version { get; private set;}

    internal NotPermittedToOperate(Vehicle aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Version = aggregate.Events.Count();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}
