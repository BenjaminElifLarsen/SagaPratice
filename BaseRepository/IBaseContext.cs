using Common.Events.Store.Event;
using Common.Events.System;
using Common.ProcessManager;

namespace Common.RepositoryPattern;
public interface IBaseContext //not tested yet against ORMs, only in-memory collections
{ //most likely will be redesigned over time
    public int Save();
    public IEnumerable<SystemEvent> SystemEvents { get; }
    public bool Filter { get; set; }

    public void Add(IAggregateRoot root);
    public void Update(IAggregateRoot root);
    public void Remove(IAggregateRoot root);
    public IEnumerable<IAggregateRoot> GetTracked { get; }
    public void Add(SystemEvent @event);
    public void Remove(SystemEvent @event);
    //public void AddEvents(IAggregateRoot root);
    //public IEnumerable<Event> LoadStream(Guid id, string aggregateRoot);
    public void Add(IBaseProcessManager processManager);
    public void Remove(IBaseProcessManager processManager);
    public Task<IBaseProcessManager> LoadProcessManagerAsync(Guid correlationId);
    public IEnumerable<T> Set<T>();
}
