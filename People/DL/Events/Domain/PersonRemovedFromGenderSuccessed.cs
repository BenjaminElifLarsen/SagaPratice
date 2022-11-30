using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public class PersonRemovedFromGenderSuccessed : IDomainEventSuccess<PersonRemovedFromGenderData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public PersonRemovedFromGenderData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal PersonRemovedFromGenderSuccessed(Gender aggregate, int personId, int version, Guid correlationId, Guid causationId)
    { 
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.GenderId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(personId, aggregate.GenderId);
    }
}

public class PersonRemovedFromGenderData
{
    public int PersonId { get; set; }
    public int GenderId { get; set; }

    public PersonRemovedFromGenderData(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}
