﻿using Common.CQRS.Queries;
using Common.RepositoryPattern;
using System.Linq.Expressions;

namespace BaseRepository;

public class MockBaseRepository<TEntity, TContext> : IBaseRepository<TEntity> where TEntity : class, IAggregateRoot where TContext : IContext<TEntity>
{
    private readonly TContext _context;
    public MockBaseRepository(TContext context)
    {
        _context = context;
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
        return await Task.Run<IEnumerable<TProjection>>(() => _context.GetAll.AsQueryable().Select(query.Map()).ToArray());
    }

    public async Task<IEnumerable<TProjection>> AllByPredicateAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        return await Task.Run<IEnumerable<TProjection>>(() => _context.GetAll.AsQueryable().Where(predicate).Select(query.Map()).ToArray());
    }

    public async Task<IEnumerable<TEntity>> AllByPredicateForOperationAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        return await Task.Run<IEnumerable<TEntity>>(() => _context.GetAll.AsQueryable().Where(predicate).ToArray());
    }

    public async Task<TProjection> FindByPredicateAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, BaseQuery<TEntity, TProjection> query) where TProjection : BaseReadModel
    {
        return await Task.Run<TProjection>(() => _context.GetAll.AsQueryable().Where(predicate).Select(query.Map()).SingleOrDefault());
    }

    public async Task<TEntity> FindByPredicateForOperationAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        return await Task.Run<TEntity>(() => _context.GetAll.AsQueryable().Where(predicate).SingleOrDefault());
    }

    public async Task<bool> IsUniqueAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Task.Run(() => !_context.GetAll.AsQueryable().Any(predicate));
    }

    public int SaveChanges()
    {
        //if (_entities.Any(x => x.State == States.Unknown))
        //{
        //    throw new Exception("Entity in unknown state.");
        //}
        //if (_entities.Where(x => x.State == States.Remove && x.Entity is ISoftDeleteDate e && e.DeletedFrom is null).Any())
        //{
        //    throw new Exception("ISoftDeleteDate entity deleted incorrectly, call void Delete(DateOnly) method.");
        //}
        //foreach (var entity in _entities.Where(x => x.State == States.Remove && x.Entity is ISoftDeleteDate e))
        //{
        //    entity.State = States.Update;
        //}
        return _context.Save();
    }

    public Task<IEnumerable<TEntity>> AllTrackedEntities()
    {
        return Task.Run(() => _context.GetAllTracked);
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

    //public override bool Equals(object obj)
    //{
    //    if(obj is not EntityState<T>) return false;

    //    EntityState<T> o = obj as EntityState<T>;

    //    return Entity.Equals(o.Entity);
    //}

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