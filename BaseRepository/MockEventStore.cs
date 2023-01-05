using Common.Events.Store.Event;
using Common.RepositoryPattern;

namespace BaseRepository;
public class MockEventStore : IEventStore
{ //current version does not implement memento pattern, implement when current version is well tested and fully working.
    private readonly IList<Event> _events;
    private readonly IList<Aggregate> _aggregates;
    private readonly IList<Snapshot> _snapshots;

    private readonly ushort _amountOfEventsBeforeSnapshop = 10;

    public MockEventStore()
    {
        _events = new List<Event>();
        _aggregates = new List<Aggregate>();
        _snapshots = new List<Snapshot>();
    }


    public void AddEvents(IAggregateRoot aggregate)
    {
        var entity = _aggregates.SingleOrDefault(x => x.AggregateId == aggregate.Id && x.Type == aggregate.GetType().Name);

        if(entity is null)
        {
            entity = new(aggregate);
            _aggregates.Add(entity);
        }
        //need to handle inconsistence of event ids and expected id, e.g. aggregate.Version = 10, first event id = 12
        //or 11 and next is 15
        //errors can also happen if two different commands are modifying the same entity at the same time
        foreach(var @event in aggregate.Events.Where(x => x.Version > entity.Version).Select(x => new Event(x)).ToList())
        {
            _events.Add(@event);
        }
        entity.UpdateVersion(aggregate.Events.OrderBy(x => x.Version).Last().Version);
    }

    public async Task<IEnumerable<Event>> LoadStreamAsync(Guid id, string aggregateType)
    {
        return await Task.Run<IEnumerable<Event>>(() => _events.Where(x => x.AggregateId == id && x.AggregateType == aggregateType).OrderBy(x => x.Version));
    }

    public async Task<IEnumerable<Event>> LoadStreamAsync(Guid id, string aggregateType, DateTime endTime)
    {
        var tick = endTime.Ticks;
        return await Task.Run(() => _events.Where(x => x.AggregateId == id && x.AggregateType == aggregateType && x.Timestamp <= tick).OrderBy(x => x.Version));
    }

    //public void Save()
    //{
    //    throw new NotImplementedException();
    //}
}
