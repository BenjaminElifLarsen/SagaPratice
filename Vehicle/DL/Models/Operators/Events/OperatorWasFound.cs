using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorWasFound : IDomainEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public OperatorWasFound(int operatorId, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(Operator).Name;
        AggregateId = operatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
    }
}
