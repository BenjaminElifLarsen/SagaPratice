using Common.CQRS.Queries;
using Common.RepositoryPattern;
using System.Linq.Expressions;

namespace BaseRepository;

public class MockBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IAggregateRoot
{
    private static IEnumerable<TEntity> _entities = new List<TEntity>();

    public MockBaseRepository()
    {

    }

    public Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TProjection>> AllByPredicateAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> AllByPredicateForOperationAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    public void Create(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TProjection> FindByPredicateAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> FindByPredicateForOperationAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUniqueAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public int SaveChanges()
    {
        throw new NotImplementedException();
    }

    public void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }
}