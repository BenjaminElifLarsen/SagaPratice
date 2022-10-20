using BaseRepository;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.People;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
internal class MockVehicleContext : IContext<Vehicle>, IContext<LicenseType>, IContext<VehicleInformation>, IContext<Person>
{
    private HashSet<Vehicle> _vehicles;
    public HashSet<Vehicle> Vehicles => _vehicles;

    private HashSet<LicenseType> _licenseTypes;
    public HashSet<LicenseType> LicenseTypes => _licenseTypes;

    private HashSet<VehicleInformation> _vehicleInformation;
    public HashSet<VehicleInformation> VehicleInformations => _vehicleInformation;

    private HashSet<Person> _people;
    public HashSet<Person> People => _people;

    IEnumerable<Vehicle> IContext<Vehicle>.GetAll => Vehicles;

    IEnumerable<LicenseType> IContext<LicenseType>.GetAll => LicenseTypes;

    IEnumerable<VehicleInformation> IContext<VehicleInformation>.GetAll => VehicleInformations;

    IEnumerable<Person> IContext<Person>.GetAll => People;

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

    public void Add(IEnumerable<Person> entities)
    {
        AddToCollection(_people, entities, x => entities.Any(xx => x.PersonId == xx.PersonId));
    }

    public void Update(IEnumerable<Person> entities)
    {
        if (entities.Any(x => !_people.Any(xx => x.PersonId == xx.PersonId)))
        {
            throw new Exception("Trying to add entity in update.");
        }
        foreach (var entity in entities)
        {
            _people.RemoveWhere(x => x.PersonId == entity.PersonId);
            _people.Add(entity);
        }
    }

    public void Remove(IEnumerable<Person> entities)
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
