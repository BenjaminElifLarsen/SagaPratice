using BaseRepository;
using Common.Events.Domain;
using Common.RepositoryPattern;
using PeopleDomain.DL.Model;

namespace PeopleDomain.IPL.Context;
internal class MockPeopleContext : IPeopleContext
{
    private readonly HashSet<EntityState<IAggregateRoot>> _contextData;
    private DateOnly _date; //mayhaps move the data out into its own class that can be set as singleton. EF Core context is scoped and it could be nice if this context was slightly more similar to it on that point.
    
    public bool Filter { get; set; }

    private Func<TEntity, bool> Filtering<TEntity>()
    {
        return x => {
            if (!Filter) return true;
            else if (x is ISoftDelete delete) return !delete.Deleted;
            else if (x is ISoftDeleteDate date) return date.DeletedFrom is null || _date < date.DeletedFrom;
            else return true;
        };
    }

    public IEnumerable<Gender> Genders => _contextData.Where(x => x.Entity is Gender).Select(x => x.Entity as Gender);
    public IEnumerable<Person> People => _contextData.Where(x => x.Entity is Person).Select(x => x.Entity as Person);
    IEnumerable<Gender> IContextData<Gender>.GetAll => Genders.Where(Filtering<Gender>());
    IEnumerable<Person> IContextData<Person>.GetAll => People.Where(Filtering<Person>());
    public IEnumerable<IAggregateRoot> GetAllTrackedEntities => _contextData.Select(x => x.Entity);
    public IEnumerable<IAggregateRoot> AllTrackedEntities => _contextData.Select(x => x.Entity);
    public IEnumerable<IDomainEvent> AllTrackedEvents => _contextData.SelectMany(x => x.Entity.Events);

    public IEnumerable<IDomainEvent> Events => _contextData.SelectMany(x => x.Entity.Events);

    public IEnumerable<IAggregateRoot> GetTracked => _contextData.Select(x => x.Entity).ToArray();

    public MockPeopleContext()
    {
        var dateTime = DateTime.Now;
        _date = new(dateTime.Year, dateTime.Month, dateTime.Day);
        _contextData = new();
        Filter = true;
    }

    public void Add(IAggregateRoot root)
    { //check if the entity is already present
        if (!_contextData.Any(x => x.Entity == root))
            _contextData.Add(new(root,States.Add));
    }

    public void Update(IAggregateRoot root)
    {
        var entity = _contextData.SingleOrDefault(x => x.Entity == root);
        if (entity is not null)
        {
            entity.State = States.Update;
        }
    }

    public void Remove(IAggregateRoot root)
    {
        var entity = _contextData.SingleOrDefault(x => x.Entity == root);
        if(entity is not null)
        {
            entity.State = States.Remove;
        }
    }

    public int SaveChanges() { 
        return Save();
    }

    public int Save()
    {
        int amount = _contextData.Count(x => x.State != States.Tracked);
        Update();
        Add();
        Remove();
        return amount;
    }

    public void Update()
    {
        var entitiesToUpdate = _contextData.Where(x => x.State == States.Update).ToArray();
        for(int i = 0; i < entitiesToUpdate.Length; i++)
        {
            entitiesToUpdate[i].State = States.Tracked;
        }
    }

    public void Add()
    {
        var entitesToAdd = _contextData.Where(x => x.State == States.Add).ToArray();
        for(int i = 0; i < entitesToAdd.Length; i++)
        {
            entitesToAdd[i].State = States.Tracked;
        }
    }

    public void Remove()
    {
        var entitiesToRemove = _contextData.Where(x => x.State == States.Remove).ToArray();
        for(int i = 0; i < entitiesToRemove.Length; i++)
        {
            entitiesToRemove[i].State = States.Tracked;
            if (entitiesToRemove[i].Entity is ISoftDelete s)
            {
                s.Delete();
            }
            else if (entitiesToRemove[i].Entity is ISoftDeleteDate sd)
            {
                if(sd.DeletedFrom is null)
                    sd.Delete(_date);
            }
            else
            {
                _contextData.Remove(entitiesToRemove[i]);
            }
        }
    }
}
