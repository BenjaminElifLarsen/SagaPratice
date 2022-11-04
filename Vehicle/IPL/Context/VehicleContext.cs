using BaseRepository;
using Common.RepositoryPattern;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
internal class MockVehicleContext : IContext<Vehicle>, IContext<LicenseType>, IContext<VehicleInformation>, IContext<Operator>
{
    private readonly HashSet<Vehicle> _vehicles;
    public HashSet<Vehicle> Vehicles => _vehicles;

    private readonly HashSet<LicenseType> _licenseTypes;
    public HashSet<LicenseType> LicenseTypes => _licenseTypes;

    private readonly HashSet<VehicleInformation> _vehicleInformation;
    public HashSet<VehicleInformation> VehicleInformations => _vehicleInformation;

    private readonly HashSet<Operator> _people;
    public HashSet<Operator> People => _people;

    private DateOnly _date;


    IEnumerable<Vehicle> IContext<Vehicle>.GetAll => Vehicles.Where(Filtering<Vehicle>());

    IEnumerable<LicenseType> IContext<LicenseType>.GetAll => LicenseTypes.Where(Filtering<LicenseType>());

    IEnumerable<VehicleInformation> IContext<VehicleInformation>.GetAll => VehicleInformations.Where(Filtering<VehicleInformation>());

    IEnumerable<Operator> IContext<Operator>.GetAll => People.Where(Filtering<Operator>());

    private Func<TEntity,bool> Filtering<TEntity>() 
    {
        return x => {
        if (!Filter) return true;
        else if (x is ISoftDelete delete) return !delete.Deleted;
        else if (x is ISoftDeleteDate date) return date.DeletedFrom is null || _date<date.DeletedFrom;
        else return true; };
    }

    public bool Filter { get; set; }

    public MockVehicleContext()
    {
        var dateTime = DateTime.Now; //would be better to have a method that calculates and returns the current date as this could cause a problem if operated around midnight. 
        _date = new(dateTime.Year, dateTime.Month, dateTime.Day);
        _vehicles = new();
        _people = new();
        _licenseTypes = new();
        _vehicleInformation = new();
        Filter = true;
    }

    public void Add(IEnumerable<Vehicle> entities)
    {
        AddToCollection(_vehicles, entities, x => entities.Any(xx => x.VehicleId == xx.VehicleId));
    }

    public void Update(IEnumerable<Vehicle> entities)
    {
        if(entities.Any(x => !_vehicles.Any(xx => x.VehicleId == xx.VehicleId)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach (var entity in entities)
        {
            _vehicles.RemoveWhere(x => x.VehicleId == entity.VehicleId);
            _vehicles.Add(entity);
        }
    }

    public void Remove(IEnumerable<Vehicle> entities)
    {
        RemoveFromCollection(_vehicles, entities);
    }

    public void Add(IEnumerable<Operator> entities)
    {
        AddToCollection(_people, entities, x => entities.Any(xx => x.OperatorId == xx.OperatorId));
    }

    public void Update(IEnumerable<Operator> entities)
    {
        if (entities.Any(x => !_people.Any(xx => x.OperatorId == xx.OperatorId)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach (var entity in entities)
        {
            _people.RemoveWhere(x => x.OperatorId == entity.OperatorId);
            _people.Add(entity);
        }
    }

    public void Remove(IEnumerable<Operator> entities)
    {
        RemoveFromCollection(_people, entities);
    }

    public void Add(IEnumerable<VehicleInformation> entities)
    {
        AddToCollection(_vehicleInformation,entities,x => entities.Any(xx => x.VehicleInformationId == xx.VehicleInformationId));
    }

    public void Update(IEnumerable<VehicleInformation> entities)
    {
        if (entities.Any(x => !_vehicleInformation.Any(xx => x.VehicleInformationId == xx.VehicleInformationId)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach(var entity in entities)
        {
            _vehicleInformation.RemoveWhere(x => x.VehicleInformationId == entity.VehicleInformationId);
            _vehicleInformation.Add(entity);
        }
    }

    public void Remove(IEnumerable<VehicleInformation> entities)
    {
        RemoveFromCollection(_vehicleInformation, entities);
    }

    public void Add(IEnumerable<LicenseType> entities)
    {
        AddToCollection(_licenseTypes, entities, x => entities.Any(xx => x.LicenseTypeId == xx.LicenseTypeId));
    }

    public void Update(IEnumerable<LicenseType> entities)
    {
        if (entities.Any(x => !_licenseTypes.Any(xx => x.LicenseTypeId == xx.LicenseTypeId)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach (var entity in entities)
        {
            _licenseTypes.RemoveWhere(x => x.LicenseTypeId == entity.LicenseTypeId);
            _licenseTypes.Add(entity);
        }
    }

    public void Remove(IEnumerable<LicenseType> entities)
    {
        RemoveFromCollection(_licenseTypes, entities);
    }


    private static void AddToCollection<T>(HashSet<T> collection, IEnumerable<T> entities, Expression<Func<T,bool>> predicate)
    {
        if (collection.AsQueryable().Any(predicate))
        {
            throw new Exception("Entity already present.");
        }
        foreach(var entity in entities)
        {
            collection.Add(entity);
        }
    }

    private static void RemoveFromCollection<T>(HashSet<T> collection, IEnumerable<T> entities)
    {
        foreach(var entity in entities)
        {
            collection.Remove(entity);
        }
    }

}
