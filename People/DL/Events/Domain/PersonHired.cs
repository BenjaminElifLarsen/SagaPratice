using Common.Events.Domain;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Events.Domain;
public class PersonHired : IDomainEvent<PersonData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public PersonData Data { get; private set; }

    internal PersonHired(Person aggregate)
    { //instead of PersonData data, could just create an instance out of Person.
        //domain events are supposed to be triggered before saving, which means an ORM cannot assign an Id, so will need to do that 'manual'.
        //mayhaps factory or handler should assign an id and validate it against the context?
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = "Person-Hired";
        TimeStampRecorded = DateTime.Now.Ticks;
        Data = new(aggregate.PersonId,aggregate.Gender.Id);
    }
}

/// <summary>
/// Model used to transmit important data for use inside of the domain. Not all receivers may use all data.
/// </summary>
public class PersonData
{
    public int PersonId { get; set; }
    public int GenderId { get; set; }

    public PersonData(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}
