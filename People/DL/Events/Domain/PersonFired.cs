using Common.Events.Domain;

namespace PeopleDomain.DL.Events.Domain;
public class PersonFired : IDomainEvent<PersonFiredData>
{
    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();

    public PersonFiredData Data => throw new NotImplementedException();
}

public class PersonFiredData
{

}