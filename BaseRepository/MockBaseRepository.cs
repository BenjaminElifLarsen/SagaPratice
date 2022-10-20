﻿using Common.CQRS.Queries;
using Common.RepositoryPattern;
using System.Linq;
using System.Linq.Expressions;

namespace BaseRepository;

public class MockBaseRepository<TEntity, TContext> : IBaseRepository<TEntity> where TEntity : class, IAggregateRoot where TContext : IContext<TEntity>
{
    /// <summary>
    /// The collection hold data not yet to be added to the context.
    /// If the id already exist over in the context it will be overwitten (a kind of 'update')
    /// </summary>
    private static IList<EntityState<TEntity>> _entities = new List<EntityState<TEntity>>();
    private readonly TContext _context;
    public MockBaseRepository(TContext context)
    {
        _context = context;
    }

    public void Create(TEntity entity)
    {
        _entities.Add(new(entity, States.Add));
    }

    public void Update(TEntity entity)
    {
        _entities.Add(new(entity, States.Update));
    }

    public void Delete(TEntity entity)
    {
        _entities.Add(new(entity, States.Remove)); //need to check if it is already in another state, same for the other methods. Also need to deal with children. In cases of 1/m-m references can permit the 'deleted' entity to be found.
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
        return !await Task.Run(() => _context.GetAll.AsQueryable().Any(predicate));
    }

    public int SaveChanges()
    {
        if (_entities.Any(x => x.State == States.Unknown))
            throw new Exception("Entity in unknown state.");
        _context.Add(_entities.Where(x => x.State == States.Add).Select(x => x.Entity));
        _context.Add(_entities.Where(x => x.State == States.Update).Select(x => x.Entity));
        _context.Add(_entities.Where(x => x.State == States.Remove).Select(x => x.Entity));
        int amount = _entities.Count;
        _entities.Clear();
        return amount;
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