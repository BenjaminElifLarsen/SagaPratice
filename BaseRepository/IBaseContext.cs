using Common.Events.Domain;

namespace Common.RepositoryPattern;
public interface IBaseContext
{
    public int Save();
    public IEnumerable<IDomainEvent> OrphanEvents { get; }
    public bool Filter { get; set; }

    public void Add(IAggregateRoot root);
    public void Update(IAggregateRoot root);
    public void Remove(IAggregateRoot root);
    public IEnumerable<IAggregateRoot> GetTracked { get; }
    public void Add(IDomainEvent @event);
    public void Remove(IDomainEvent @event);
}
