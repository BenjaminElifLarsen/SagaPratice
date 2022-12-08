using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
internal class VehicleNotRequiredToRemoveOperator : IDomainEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public VehicleNotRequiredToRemoveOperator(Vehicle aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks; 
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = aggregate.Events.Count();
    }
}
