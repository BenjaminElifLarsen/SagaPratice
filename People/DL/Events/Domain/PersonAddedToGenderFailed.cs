using Common.Events.Domain;

namespace PeopleDomain.DL.Events.Domain;
public class PersonAddedToGenderFailed : IDomainEventFail
{
    public IEnumerable<string> Errors => throw new NotImplementedException();

    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();

    public Guid CorrelationId => throw new NotImplementedException();

    public Guid CausationId => throw new NotImplementedException();
}
