using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
internal interface IPersonConversion
{
    public static abstract DomainEvent Set(Event @event);
    public static abstract IEnumerable<DataPoint> Get(PersonHiredSucceeded @event);
    public static abstract IEnumerable<DataPoint> Get(PersonFiredSucceeded @event);
    public static abstract IEnumerable<DataPoint> Get(PersonPersonalInformationChangedSuccessed @event);
    public static abstract IEnumerable<DataPoint> Get(PersonChangedGender @event);

}
