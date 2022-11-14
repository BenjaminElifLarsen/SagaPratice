using Common.RepositoryPattern;

namespace BaseRepository;
public interface IContext<TEntity> where TEntity : IAggregateRoot
{
    public IEnumerable<TEntity> GetAll { get; }
    public bool Filter { get; set; } //ensure there is one for each aggregate root
    public int Save();
    public void Add(IAggregateRoot root);
    public void Update(IAggregateRoot root);
    public void Remove(IAggregateRoot root);
    public IEnumerable<IAggregateRoot> GetAllTrackedEntities { get; }
    public IEnumerable<TEntity> GetAllTracked { get; }
}
