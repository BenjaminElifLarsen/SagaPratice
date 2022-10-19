using Common.CQRS.Queries;
using Common.RepositoryPattern;
using System.Linq.Expressions;

namespace BaseRepository;

public class MockBaseRepository<TEntity, TContext> : IBaseRepository<TEntity> where TEntity : class, IAggregateRoot where TContext : IContext<TEntity>
{
    /// <summary>
    /// The collection hold data not yet to be added to the context.
    /// If the id already exist over in the context it will be overwitten (a kind of 'update')
    /// </summary>
    private static IList<EntityState<TEntity>> _entities = new List<EntityState<TEntity>>();

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
        _entities.Add(new(entity, States.Add));
    }

    public void Delete(TEntity entity)
    {
        _entities.Add(new(entity, States.Remove));
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
        _entities.Add(new(entity, States.Update));
    }

    public class EntityState<T>
    {
        public States State { get; set; }
        public T Entity { get; set; }
        public EntityState(T entity, States state)
        {
            Entity = entity;
            State = state;
        }
    }

    public enum States
    {
        Add = 1,
        Update = 2,
        Remove = 3,

        Unknown = 0
    }
}