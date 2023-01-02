using Common.Events.Store.Event;

namespace PeopleDomain.IPL.Context;
internal class PeopleEventContext : IEventStore
{
    private readonly IEnumerable<Event> _events;
    private readonly IEnumerable<Aggregate> _aggregates;

}
