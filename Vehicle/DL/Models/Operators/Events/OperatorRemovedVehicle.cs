using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorRemovedVehicle : IDomainEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }
    
    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal OperatorRemovedVehicle(Operator aggregate, Guid correlationId, Guid causationId)
    { //no reason to have int version as a parameter as it can be got from the aggregate itself
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = aggregate.OldEventsDesign.Count();
    }
}
