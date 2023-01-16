using Common.Events.Store.Event;
using Common.RepositoryPattern.Events;

namespace BaseRepository;
public class MockEventRepository<TId, TBaseContext> : IBaseEventRepository<TId> where TBaseContext : IEventStore<TId>
{
    private readonly TBaseContext _context;
    
    public MockEventRepository(TBaseContext context)
    {
        _context = context;
    }

    //public void AddEvent(DomainEvent @event)
    //{
    //    throw new NotImplementedException();
    //}

    //public void AddEvents(IAggregateRoot root)
    //{
    //    //only need to add the events after the last add
    //    throw new NotImplementedException();
    //}

    public void AddEvents(IEnumerable<Event<TId>> events)
    {
        foreach(var e in events)
        {
            _context.AddEvent(e);
        }
    }

    public async Task<IEnumerable<Event<TId>>> LoadEntityEventsAsync(TId id, string aggregateType)
    {
        return await _context.LoadStreamAsync(id, aggregateType);
    }

    public async Task<IEnumerable<Event<TId>>> LoadEntityEventsUptoAsync(TId id, string aggregateType, DateTime UpTo)
    {
        return await _context.LoadStreamAsync(id, aggregateType, UpTo);
    }

    public async Task<IEnumerable<Event<TId>>> LoadAllEvents(string aggregateType)
    {
        return await _context.LoadStreamAsync(aggregateType);
    }

}
