using System.Collections;

namespace BaseRepository;
public class DataSet<T> : IEnumerable<T>
{ //as nice this could be, it woill be difficult to make it work with entiy frame work
    //will require to get the DbSet<T> from entity framework core and convert it to this while making sure it can still store in the context.
    //maybe just keep this for mock-ups and update the base context contract to take in what type of dbset it should return
    private readonly HashSet<T> _set; //dbset should be able to be arquired via the context.Set<T>

    public DataSet(IEnumerable<T> data)
    {
        _set = new(data);
    }

    public void Add(T entity)
    {
        _set.Add(entity);
    }

    public void Remove(T entity)
    {
        _set.Remove(entity);
    }

    public void Update(T entity)
    {
        //not really needed right now, implement later
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _set.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _set.GetEnumerator();
    }
}
