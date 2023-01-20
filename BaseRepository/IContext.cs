using Common.DDD;

namespace BaseRepository;
public interface IContextData<TEntity> where TEntity : IAggregateRoot
{
    //public IEnumerable<TEntity> GetAll { get; }
}
