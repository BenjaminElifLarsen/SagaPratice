using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public class PersonChangedGender : IDomainEvent<PersonChangedGenderData>
{
    public PersonChangedGenderData Data { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal PersonChangedGender(Person aggregate, int oldGenderId, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.PersonId, aggregate.Gender.Id, oldGenderId);
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
    }
}

public class PersonChangedGenderData
{
    public int PersonId { get; private set; }
    public int NewGenderId { get; private set; }
    public int OldGenderId { get; private set; }

    public PersonChangedGenderData(int personId, int newGenderId, int oldGenderId)
    {
        PersonId = personId;
        NewGenderId = newGenderId;
        OldGenderId = oldGenderId;
    }
}