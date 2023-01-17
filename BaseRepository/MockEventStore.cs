using Common.Events.Store.Event;
using Common.RepositoryPattern;

namespace BaseRepository;
public class MockEventStore : IEventStore<Guid>
{ //current version does not implement memento pattern, implement when current version is well tested and fully working.
    private readonly IList<Event<Guid>> _events;
    private readonly IList<Aggregate> _aggregates;
    private readonly IList<Snapshot> _snapshots;

    private readonly ushort _amountOfEventsBeforeSnapshop = 10;
    public IEnumerable<Event<Guid>> DeleteWhenDoneWithTestingAnIdeaThanksALot => _events;
    public MockEventStore()
    {
        _events = new List<Event<Guid>>();
        _aggregates = new List<Aggregate>();
        _snapshots = new List<Snapshot>();
    }

    public void AddEvent(Event<Guid> @event)
    {
        var entity = GetAggregate(@event);
        @event.SetVersion(entity.Version + 1);
        _events.Add(@event); //not sure the current way to set Common.Vents.Store.Event version is the best as it depends on the IAggregateRoot event collection 
        entity.UpdateVersion(@event.Version);
    }

    public void AddEvents(IEnumerable<Event<Guid>> events)
    {
        throw new NotImplementedException();
    }

    private Aggregate GetAggregate(Event<Guid> @event)
    {
        var entity = _aggregates.SingleOrDefault(x => x.AggregateId == @event.AggregateId && x.Type == @event.AggregateType);
        if (entity is null)
        {
            entity = new(@event.AggregateId, @event.AggregateType);
            _aggregates.Add(entity);
        }
        return entity;
    }


    //public void AddEvents(IAggregateRoot aggregate)
    //{
    //    var entity = _aggregates.SingleOrDefault(x => x.AggregateId == aggregate.Id && x.Type == aggregate.GetType().Name);

    //    if(entity is null)
    //    {
    //        entity = new(aggregate);
    //        _aggregates.Add(entity);
    //    }
    //    //need to handle inconsistence of event ids and expected id, e.g. aggregate.Version = 10, first event id = 12
    //    //or 11 and next is 15
    //    //errors can also happen if two different commands are modifying the same entity at the same time
    //    foreach(var @event in aggregate.Events.Where(x => x.Version > entity.Version).Select(x => new Event(x)).ToList())
    //    {
    //        _events.Add(@event);
    //    }
    //    entity.UpdateVersion(aggregate.Events.OrderBy(x => x.Version).Last().Version);
    //}

    public async Task<IEnumerable<Event<Guid>>> LoadStreamAsync(Guid id, string aggregateType)
    {
        return await Task.Run(() => _events.Where(x => x.AggregateId == id && x.AggregateType == aggregateType).OrderBy(x => x.Version));
    }

    public async Task<IEnumerable<Event<Guid>>> LoadStreamAsync(Guid id, string aggregateType, DateTime endTime)
    {
        var tick = endTime.Ticks;
        return await Task.Run(() => _events.Where(x => x.AggregateId == id && x.AggregateType == aggregateType && x.Timestamp <= tick).OrderBy(x => x.Version));
    }

    public async Task<IEnumerable<Event<Guid>>> LoadStreamAsync(string aggregateType)
    {
        return await Task.Run(() => _events.Where(x => x.AggregateType == aggregateType).OrderBy(x => x.Version));
    }

    //public void Save()
    //{
    //    throw new NotImplementedException();
    //}
}
