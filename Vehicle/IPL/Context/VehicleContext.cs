using BaseRepository;
using Common.RepositoryPattern;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
internal class MockVehicleContext : IContext<Vehicle>, IContext<LicenseType>, IContext<VehicleInformation>, IContext<Operator>
{
    private HashSet<Vehicle> _vehicles;
    public HashSet<Vehicle> Vehicles => _vehicles;

    private HashSet<LicenseType> _licenseTypes;
    public HashSet<LicenseType> LicenseTypes => _licenseTypes;

    private HashSet<VehicleInformation> _vehicleInformation;
    public HashSet<VehicleInformation> VehicleInformations => _vehicleInformation;

    private HashSet<Operator> _people;
    public HashSet<Operator> People => _people;

    IEnumerable<Vehicle> IContext<Vehicle>.GetAll => Vehicles.Where(x => { 
        if (x is ISoftDelete delete) return !delete.Deleted; 
        else if (x is ISoftDeleteDate date) return date.DeletedFrom < DateTime.Now; 
        else return true; }
    );

    IEnumerable<LicenseType> IContext<LicenseType>.GetAll => LicenseTypes.Where(x => {
        if (x is ISoftDelete delete) return !delete.Deleted;
        else if (x is ISoftDeleteDate date) return date.DeletedFrom < DateTime.Now;
        else return true; }
    );

    IEnumerable<VehicleInformation> IContext<VehicleInformation>.GetAll => VehicleInformations.Where(x => {
        if (x is ISoftDelete delete) return !delete.Deleted;
        else if (x is ISoftDeleteDate date) return date.DeletedFrom < DateTime.Now;
        else return true; }
    );

    IEnumerable<Operator> IContext<Operator>.GetAll => People.Where(x => {
        if (x is ISoftDelete delete) return !delete.Deleted;
        else if (x is ISoftDeleteDate date) return date.DeletedFrom < DateTime.Now;
        else return true; }
    );

    public MockVehicleContext()
    {
        _vehicles = new();
        _people = new();
        _licenseTypes = new();
        _vehicleInformation = new();
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
