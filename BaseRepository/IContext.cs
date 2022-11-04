namespace BaseRepository;
public interface IContext<TEntity>
{
    public void Add(IEnumerable<TEntity> entities);
    public void Update(IEnumerable<TEntity> entities);
    public void Remove(IEnumerable<TEntity> entities);
    public IEnumerable<TEntity> GetAll { get; }
    public bool Filter { get; set; } //ensure there is one for each aggregate root
}
