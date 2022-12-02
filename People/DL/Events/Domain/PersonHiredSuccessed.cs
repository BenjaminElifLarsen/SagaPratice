using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed class PersonHiredSuccessed : IDomainEvent<PersonHiredData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public PersonHiredData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal PersonHiredSuccessed(Person aggregate, int version, Guid correlationId, Guid causationId)
    { //instead of PersonData data, could just create an instance out of Person.
        //domain events are supposed to be triggered before saving, which means an ORM cannot assign an Id, so will need to do that 'manual'.
        //mayhaps factory or handler should assign an id and validate it against the context?
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.PersonId, aggregate.Gender.Id);
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
    }
}

/// <summary>
/// Model used to transmit important data for use inside of the domain. Not all receivers may use all data.
/// </summary>
public class PersonHiredData
{
    public int PersonId { get; private set; }
    public int GenderId { get; private set; }

    public PersonHiredData(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}
