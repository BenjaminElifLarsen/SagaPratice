using Common.RepositoryPattern;

namespace BaseRepository;
public interface IContextData<TEntity> where TEntity : IAggregateRoot
{
    public IEnumerable<TEntity> GetAll { get; }
}
