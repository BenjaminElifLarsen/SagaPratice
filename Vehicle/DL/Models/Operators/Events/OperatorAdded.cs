using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public class OperatorAdded : IDomainEvent
{
    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();
}
