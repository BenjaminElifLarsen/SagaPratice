using BaseRepository;
using Common.RepositoryPattern;
using PeopleDomain.DL.Model;
using System.Linq.Expressions;

namespace PeopleDomain.IPL.Context;
internal class MockPeopleContext : IContext<Person>, IContext<Gender>
{
    private readonly HashSet<Gender> _genders;
    public HashSet<Gender> Genders => _genders;

    private readonly HashSet<Person> _people;
    public HashSet<Person> People => _people;

    private DateOnly _date;

    IEnumerable<Gender> IContext<Gender>.GetAll => Genders.Where(Filtering<Gender>());

    IEnumerable<Person> IContext<Person>.GetAll => People.Where(Filtering<Person>());

    private Func<TEntity, bool> Filtering<TEntity>()
    {
        return x => {
            if (!Filter) return true;
            else if (x is ISoftDelete delete) return !delete.Deleted;
            else if (x is ISoftDeleteDate date) return date.DeletedFrom is null || _date < date.DeletedFrom;
            else return true;
        };
    }

    public bool Filter { get; set; }

    public MockPeopleContext()
    {
        var dateTime = DateTime.Now;
        _date = new(dateTime.Year, dateTime.Month, dateTime.Day);
        _genders = new();
        _people = new();
        Filter = true;
    }

    public void Add(IEnumerable<Gender> entities)
    {
        AddToCollection(_genders, entities, x => entities.Any(xx => x == xx));
    }

    public void Add(IEnumerable<Person> entities)
    {
        AddToCollection(_people, entities, x => entities.Any(xx => x == xx));
    }

    public void Remove(IEnumerable<Gender> entities)
    {
        RemoveFromCollection(_genders, entities);
    }

    public void Remove(IEnumerable<Person> entities)
    {
        RemoveFromCollection(_people, entities);
    }

    public void Update(IEnumerable<Gender> entities)
    {
        if(entities.Any(x => !_genders.Any(y => y == x)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach(var entity in entities)
        {
            _genders.RemoveWhere(x => x == entity);
            _genders.Add(entity);
        }
    }

    public void Update(IEnumerable<Person> entities)
    {
        if(entities.Any(x => !_people.Any(y => y == x)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach(var entity in entities)
        {
            _people.RemoveWhere(x => x == entity);
            _people.Add(entity);
        }
    }

    private static void AddToCollection<T>(HashSet<T> collection, IEnumerable<T> entities, Expression<Func<T, bool>> predicate)
    {
        if (collection.AsQueryable().Any(predicate))
        {
            throw new Exception("Entity already present.");
        }
        foreach (var entity in entities)
        {
            collection.Add(entity);
        }
    }

    private static void RemoveFromCollection<T>(HashSet<T> collection, IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            collection.Remove(entity);
        }
    }
}
