using Common.Events.Domain;
using Common.Events.Store.Event;
using Common.MementoPattern;
using Common.RepositoryPattern;
using Common.RepositoryPattern.Events;

namespace BaseRepository;
public class MockEventRepository<TId, TBaseContext> : IBaseEventRepository<TId> where TBaseContext : IBaseContext
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

    public void AddEvents(IEnumerable<Event> events)
    {
        throw new NotImplementedException();
    }

    //public void AddSnapshoot(IMemento memento)
    //{
    //    throw new NotImplementedException();
    //}

    public Task<IEnumerable<Event<TId>>> LoadEntityEventsUptoAsync(TId id, DateTime UpTo)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Event<TId>>> LoadEntityEventsAsync(TId id)
    {
        throw new NotImplementedException();
    }
}
