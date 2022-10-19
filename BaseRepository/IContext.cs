namespace BaseRepository;
public interface IContext<TEntity>
{
    public void Add(IEnumerable<TEntity> entities);
    public void Update(IEnumerable<TEntity> entities);
    public void Remove(IEnumerable<TEntity> entities);
}
