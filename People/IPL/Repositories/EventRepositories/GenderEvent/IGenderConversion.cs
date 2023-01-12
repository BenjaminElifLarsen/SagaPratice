using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
internal interface IGenderConversion
{
    public static abstract DomainEvent Set(Event @event);
    public static abstract IEnumerable<DataPoint> Get(GenderRecognisedSucceeded @event);
    public static abstract IEnumerable<DataPoint> Get(GenderUnrecognisedSucceeded @event);
    public static abstract IEnumerable<DataPoint> Get(PersonAddedToGenderSucceeded @event);
    public static abstract IEnumerable<DataPoint> Get(PersonRemovedFromGenderSucceeded @event);
}
