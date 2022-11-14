using BaseRepository;
using Common.RepositoryPattern;
using PeopleDomain.DL.Model;
using System.Linq;
using System.Linq.Expressions;

namespace PeopleDomain.IPL.Context;
internal class MockPeopleContext : IContext<Person>, IContext<Gender>
{
    private readonly HashSet<EntityState<IAggregateRoot>> _contextData;
    //private readonly HashSet<EntityState<IAggregateRoot>> _notInContextData;

    //private readonly HashSet<Gender> _genders;
    public IEnumerable<Gender> Genders => _contextData.Where(x => x.Entity is Gender).Select(x => x.Entity as Gender);

    //private readonly HashSet<Person> _people;
    public IEnumerable<Person> People => _contextData.Where(x => x.Entity is Person).Select(x => x.Entity as Person);

    private DateOnly _date;

    IEnumerable<Gender> IContext<Gender>.GetAll => Genders.Where(Filtering<Gender>());

    IEnumerable<Person> IContext<Person>.GetAll => People.Where(x => x is Person).Where(Filtering<Person>());

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

    public IEnumerable<IAggregateRoot> GetAllTrackedEntities => _contextData.Select(x => x.Entity);

    IEnumerable<Gender> IContext<Gender>.GetAllTracked => GetAllTrackedEntities.Where(x => x is Gender).Select(x => x as Gender).Where(Filtering<Gender>());

    IEnumerable<Person> IContext<Person>.GetAllTracked => GetAllTrackedEntities.Where(x => x is Person).Select(x => x as Person).Where(Filtering<Person>());

    public MockPeopleContext()
    {
        var dateTime = DateTime.Now;
        _date = new(dateTime.Year, dateTime.Month, dateTime.Day);
        _contextData = new();
        Filter = true;
    }

    public void Add(IAggregateRoot root)
    {
        _contextData.Add(new(root,States.Add));
    }

    public void Update(IAggregateRoot root)
    {
        _contextData.Add(new(root, States.Update));
    }

    public void Remove(IAggregateRoot root)
    {
        _contextData.Add(new(root,States.Remove));
    }


    public int Save()
    {
        Update();
        Add();
        Remove();
        int amount = _contextData.Count(x => x.State != States.Tracked);
        return amount;
    }

    public void Update()
    {
        var entitiesToUpdate = _contextData.Where(x => x.State == States.Update).ToArray();
        for(int i = 0; i < entitiesToUpdate.Count(); i++)
        {
            _contextData.RemoveWhere(x => x.Entity == entitiesToUpdate[i].Entity);
            entitiesToUpdate[i].State = States.Tracked;
        }
    }

    public void Add()
    {
        var entitesToAdd = _contextData.Where(x => x.State == States.Add).ToArray();
        for(int i = 0; i < entitesToAdd.Count(); i++)
        {
            entitesToAdd[i].State = States.Tracked;
        }
    }

    public void Remove()
    {
        var entitiesToRemove = _contextData.Where(x => x.State == States.Remove).Where(x => _contextData.Any(y => x.Entity == y.Entity)).ToArray();
        for(int i = 0; i < entitiesToRemove.Count(); i++)
        {
            entitiesToRemove[i].State = States.Tracked;
        }
    }
}
