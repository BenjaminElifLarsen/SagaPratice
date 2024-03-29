﻿using BaseRepository;
using Common.DDD;
using Common.Events.Store.Event;
using Common.Events.System;
using Common.ProcessManager;
using Common.RepositoryPattern;
using System.Xml.Linq;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
internal class MockVehicleContext : IVehicleContext
{
    //private readonly HashSet<EntityState<IAggregateRoot>> _contextData;
    private readonly HashSet<SystemEvent> _events;
    private readonly HashSet<EntityState<object>> _data;
    private DateOnly _date;
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
    public IEnumerable<Operator> Operators => _data.Where(x => x.Entity is Operator).Select(x => x.Entity as Operator);
    public IEnumerable<VehicleInformation> VehicleInformations => _data.Where(x => x.Entity is VehicleInformation).Select(x => x.Entity as VehicleInformation);
    public IEnumerable<LicenseType> LicenseTypes => _data.Where(x => x.Entity is LicenseType).Select(x => x.Entity as LicenseType);
    public IEnumerable<Vehicle> Vehicles => _data.Where(x => x.Entity is Vehicle).Select(x => x.Entity as Vehicle);

    //IEnumerable<Operator> IContextData<Operator>.GetAll => Operators.Where(Filtering<Operator>());
    //IEnumerable<VehicleInformation> IContextData<VehicleInformation>.GetAll => VehicleInformations.Where(Filtering<VehicleInformation>());
    //IEnumerable<LicenseType> IContextData<LicenseType>.GetAll => LicenseTypes.Where(Filtering<LicenseType>());
    //IEnumerable<Vehicle> IContextData<Vehicle>.GetAll => Vehicles.Where(Filtering<Vehicle>());

    public IEnumerable<IAggregateRoot> GetTracked => Set<IAggregateRoot>();

    public IEnumerable<SystemEvent> SystemEvents => _events;

    public MockVehicleContext()
    {
        var dateTime = DateTime.Now; //would be better to have a method that calculates and returns the current date as this could cause a problem if operated around midnight. 
        _date = new(dateTime.Year, dateTime.Month, dateTime.Day);
        _data = new();
        Filter = true;
        _events = new();
    }

    public void Add(IAggregateRoot root)
    {
        if (!_data.Any(x => x.Entity == root))
            _data.Add(new(root, States.Add));
    }

    public void Update(IAggregateRoot root)
    {
        var entity = _data.SingleOrDefault(x => x.Entity == root);
        if (entity is not null)
        {
            entity.State = States.Update;
        }
    }

    public void Remove(IAggregateRoot root)
    {
        var entity = _data.SingleOrDefault(x => x.Entity == root);
        if (entity is not null)
        {
            entity.State = States.Remove;
        }
    }

    public int SaveChanges()
    {
        return Save();
    }

    public int Save()
    {
        int amount = _data.Count(x => x.State != States.Tracked);
        Update();
        Add();
        Remove();
        return amount;
    }

    public void Update()
    {
        var entitiesToUpdate = _data.Where(x => x.State == States.Update).ToArray();
        for (int i = 0; i < entitiesToUpdate.Length; i++)
        {
            entitiesToUpdate[i].State = States.Tracked;
        }
    }

    public void Add()
    {
        var entitesToAdd = _data.Where(x => x.State == States.Add).ToArray();
        for (int i = 0; i < entitesToAdd.Length; i++)
        {
            entitesToAdd[i].State = States.Tracked;
        }
    }

    public void Remove()
    {
        var entitiesToRemove = _data.Where(x => x.State == States.Remove).ToArray();
        for (int i = 0; i < entitiesToRemove.Length; i++)
        {
            entitiesToRemove[i].State = States.Tracked;
            if (entitiesToRemove[i].Entity is ISoftDelete s)
            {
                s.Delete();
            }
            else if (entitiesToRemove[i].Entity is ISoftDeleteDate sd)
            {
                if (sd.DeletedFrom is null)
                    sd.Delete(_date);
            }
            else
            {
                _data.Remove(entitiesToRemove[i]);
            }
        }
    }

    public void Add(SystemEvent @event)
    {
        _events.Add(@event);
    }

    public void Remove(SystemEvent @event)
    {
        _events.Remove(@event);
    }

    public void AddEvents(IAggregateRoot root)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Event> LoadStream(Guid id, string aggregateRoot)
    {
        throw new NotImplementedException();
    }

    public void Add(IBaseProcessManager processManager)
    {
        throw new NotImplementedException();
    }

    public void Remove(IBaseProcessManager processManager)
    {
        throw new NotImplementedException();
    }

    public Task<IBaseProcessManager> LoadProcessManagerAsync(Guid correlationId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Set<T>()
    {
        return _data.Where(x => x is T).Select(x => (T)x.Entity);
    }
}
