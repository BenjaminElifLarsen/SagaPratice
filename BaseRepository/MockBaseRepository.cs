using Common.CQRS.Queries;
using Common.DDD;
using Common.RepositoryPattern;
using System.Linq.Expressions;

namespace BaseRepository;

public class MockBaseRepository<TEntity, TBaseContext> : IBaseRepository<TEntity> where TEntity : class, IAggregateRoot where TBaseContext : IBaseContext
{
    private readonly TBaseContext _context;
    private readonly IEnumerable<TEntity> _data;
    public MockBaseRepository(TBaseContext context)
    {
        _context = context;
        _data = _context.Set<TEntity>();
    }

    public void Create(TEntity entity)
    {
        _context.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Remove(entity); //need to check if it is already in another state, same for the other methods. Also need to deal with children. In cases of 1/m-m references can permit the 'deleted' entity to be found.
    }

    public async Task<IEnumerable<TProjection>> AllAsync<TProjection>(BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        return await Task.Run<IEnumerable<TProjection>>(() => _data.AsQueryable().Select(query.Map()).ToArray());
    }

    public async Task<IEnumerable<TProjection>> AllByPredicateAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        return await Task.Run<IEnumerable<TProjection>>(() => _data.AsQueryable().Where(predicate).Select(query.Map()).ToArray());
    }

    public async Task<IEnumerable<TEntity>> AllByPredicateForOperationAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        return await Task.Run<IEnumerable<TEntity>>(() => _data.AsQueryable().Where(predicate).ToArray());
    }

    public async Task<TProjection> FindByPredicateAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        return await Task.Run<TProjection>(() => _data.AsQueryable().Where(predicate).Select(query.Map()).SingleOrDefault());
    }

    public async Task<TEntity> FindByPredicateForOperationAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        return await Task.Run<TEntity>(() => _data.AsQueryable().Where(predicate).SingleOrDefault());
    }

    public async Task<bool> IsUniqueAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Task.Run(() => !_data.AsQueryable().Any(predicate));
    }
}

public record EntityState<T>
{
    public States State { get; set; }
    public T Entity { get; set; }
    public EntityState(T entity, States state)
    {
        Entity = entity;
        State = state;
    }

    public override int GetHashCode()
    { // Overwritten to ensure HashSet work as changing states would change the hash code.
        return Entity is not null ? Entity.GetHashCode() : base.GetHashCode();
    }

}

public enum States
{
    Add = 1,
    Update = 2,
    Remove = 3,
    Tracked = 4,

    Unknown = 0
}