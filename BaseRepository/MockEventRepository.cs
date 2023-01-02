using Common.Events.Domain;
using Common.MementoPattern;
using Common.RepositoryPattern;
using Common.RepositoryPattern.Events;

namespace BaseRepository;
public class MockEventRepository<TEntity, TBaseContext> : IBaseEventRepository<TEntity> where TEntity : IAggregateRoot where TBaseContext : IBaseContext
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

    public void AddEvents(IAggregateRoot root)
    {
        //only need to add the events after the last add
        throw new NotImplementedException();
    }

    public void AddSnapshoot(IMemento memento)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetEntityAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetEntityAsync(int id, DateTime UpTo)
    {
        throw new NotImplementedException();
    }
}
