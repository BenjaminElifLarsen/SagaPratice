using Common.Events.Store.Event;
using Common.RepositoryPattern;

namespace BaseRepository;
public class MockEventStore : IEventStore
{
    private readonly IList<Event> _events;
    private readonly IList<Aggregate> _aggregates;
    private readonly IList<Snapshot> _snapshots;

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

        foreach(var @event in aggregate.GetEvents.Select(x => new Event(x)).ToList())
        {
            _events.Add(@event);
        }
    }

    public async Task<IEnumerable<Event>> LoadStreamAsync(int id, string aggregateType)
    {
        return await Task.Run<IEnumerable<Event>>(() => _events.Where(x => x.AggregateId == id && x.AggregateType == aggregateType).OrderBy(x => x.Version));
    }

    public async Task<IEnumerable<Event>> LoadStreamAsync(int id, string aggregateType, DateTime endTime)
    {
        var tick = endTime.Ticks;
        return await Task.Run(() => _events.Where(x => x.AggregateId == id && x.AggregateType == aggregateType && x.Timestamp <= tick).OrderBy(x => x.Version));
    }

    //public void Save()
    //{
    //    throw new NotImplementedException();
    //}
}
