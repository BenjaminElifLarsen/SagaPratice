using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
internal class PersonConversion : IPersonConversion
{
    public static IEnumerable<DataPoint> Get(PersonHiredSucceeded @event)
    {
        return new List<DataPoint>
        {
            new((int)PersonPropertyId.FirstName, @event.FirstName),
            new((int)PersonPropertyId.LastName, @event.LastName),
            new((int)PersonPropertyId.Birth, @event.Birth.Ticks.ToString()),
            new((int)PersonPropertyId.Gender, @event.GenderId.ToString())
        };
    }

    public static IEnumerable<DataPoint> Get(PersonFiredSucceeded @event)
    {
        return new List<DataPoint>
        {
            new((int)PersonPropertyId.Gender, @event.GenderId.ToString())
        };
    }

    public static IEnumerable<DataPoint> Get(PersonChangedBirth @event)
    {
        return new List<DataPoint>
        {
            new((int)PersonPropertyId.Birth, @event.Birth.Ticks.ToString()),
        };
    }

    public static IEnumerable<DataPoint> Get(PersonChangedLastName @event)
    {
        return new List<DataPoint>
        {
            new((int)PersonPropertyId.LastName, @event.LastName),
        };
    }

    public static IEnumerable<DataPoint> Get(PersonChangedFirstName @event)
    {
        return new List<DataPoint>
        {
            new((int)PersonPropertyId.FirstName, @event.FirstName),
        };
    }

    public static IEnumerable<DataPoint> Get(PersonChangedGender @event)
    {
        return new List<DataPoint>
        {
            new((int)PersonPropertyId.Gender, @event.GenderId.ToString())
        };
    }

    public static DomainEvent Set(Event @event)
    {
        return @event.Type switch
        {
            nameof(PersonHiredSucceeded) => new PersonHiredSucceeded(@event),
            nameof(PersonFiredSucceeded) => new PersonFiredSucceeded(@event),
            nameof(PersonChangedGender) => new PersonChangedGender(@event),
            nameof(PersonChangedBirth) => new PersonChangedBirth(@event),
            nameof(PersonChangedFirstName) => new PersonChangedFirstName(@event),
            nameof(PersonChangedLastName) => new PersonChangedLastName(@event),
            _ => throw new Exception("Unknown Event"),
        };
    }
}
