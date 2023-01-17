using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.DL.Events.Conversion;
internal class GenderConversion : IGenderConversion
{
    public static IEnumerable<DataPoint> Get(GenderRecognisedSucceeded @event)
    {
        return new List<DataPoint>
        {
            //new((int)GenderPropertyId.Id, @event.AggregateId.ToString()),
            new((int)GenderPropertyId.VerbSubject, @event.Subject),
            new((int)GenderPropertyId.VerbObject, @event.Object)
        };
    }

    public static IEnumerable<DataPoint> Get(GenderUnrecognisedSucceeded @event)
    {
        return new List<DataPoint>
        {
            //new((int)GenderPropertyId.Id, @event.AggregateId.ToString())
        };
    }

    public static IEnumerable<DataPoint> Get(PersonAddedToGenderSucceeded @event)
    {
        return new List<DataPoint>
        {
            new((int)GenderPropertyId.PersonId, @event.PersonId.ToString())
        };
    }

    public static IEnumerable<DataPoint> Get(PersonRemovedFromGenderSucceeded @event)
    {
        return new List<DataPoint>
        {
            new((int)GenderPropertyId.PersonId, @event.PersonId.ToString())
        };
    }

    public static DomainEvent Set(Event @event)
    {
        return @event.Type switch
        { //need to add data to them
            nameof(GenderRecognisedSucceeded) => new GenderRecognisedSucceeded(@event),
            nameof(GenderUnrecognisedSucceeded) => new GenderUnrecognisedSucceeded(@event),
            nameof(PersonAddedToGenderSucceeded) => new PersonAddedToGenderSucceeded(@event),
            nameof(PersonRemovedFromGenderSucceeded) => new PersonRemovedFromGenderSucceeded(@event),
            _ => throw new Exception("Unknown Event"),
        };
    }
}
