using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
internal class OperatorRemoved : IDomainEvent<OperatorRemovedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public OperatorRemovedData Data { get; private set; }

    public OperatorRemoved(Operator aggregate)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.OperatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
    }
}

internal class OperatorRemovedData
{
    public int Id { get; private set; }

    public OperatorRemovedData(int id)
    {
        Id = id;
    }
}
