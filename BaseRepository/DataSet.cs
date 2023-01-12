using System.Collections;

namespace BaseRepository;
public class DataSet<T> : IEnumerable<T>
{
    HashSet<T> set;
    public void Add(T entity)
    {
        set.Add(entity);
    }
    public void Remove(T entity)
    {
        set.Remove(entity);
    }
    public HashSet<T> All()
    {
        return set;
    }

    public void Update(T entity)
    {

    }

    public IEnumerator<T> GetEnumerator()
    {
        return set.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return set.GetEnumerator();
    }
}
