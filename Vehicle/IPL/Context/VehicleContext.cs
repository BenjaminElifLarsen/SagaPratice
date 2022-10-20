using BaseRepository;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Vehicle.DL.Models.LicenseTypes;
using Vehicle.DL.Models.People;
using Vehicle.DL.Models.VehicleInformations;
using V = Vehicle.DL.Models.Vehicles;

namespace Vehicle.IPL.Context;
internal class MockVehicleContext : IContext<V.Vehicle>, IContext<LicenseType>, IContext<VehicleInformation>, IContext<Person>
{
    private HashSet<V.Vehicle> _vehicles;
    public HashSet<V.Vehicle> Vehicles => _vehicles;

    private HashSet<LicenseType> _licenseTypes;
    public HashSet<LicenseType> LicenseTypes => _licenseTypes;

    private HashSet<VehicleInformation> _vehicleInformation;
    public HashSet<VehicleInformation> VehicleInformations => _vehicleInformation;

    private HashSet<Person> _people;
    public HashSet<Person> People => _people;

    IEnumerable<V.Vehicle> IContext<V.Vehicle>.GetAll => Vehicles;

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

    public void Add(IEnumerable<V.Vehicle> entities)
    {
            AddToCollection(_vehicles, entities, x => entities.Any(xx => x.VehicleId == xx.VehicleId));
    }

    public void Update(IEnumerable<V.Vehicle> entities)
    {
        foreach(var entity in entities.Where(x => !_vehicles.Any(xx => x.VehicleId == xx.VehicleId)))
        {
            _vehicles.Add(entity);
        }
        foreach (var entity in entities.Where(x => _vehicles.Any(xx => x.VehicleId == xx.VehicleId)))
        {
            _vehicles.RemoveWhere(x => x.VehicleId == entity.VehicleId);
            _vehicles.Add(entity);
        }
        //UpdateCollection(_vehicles, entities, x => !_vehicles.Any(xx => x.VehicleId == xx.VehicleId), x => _vehicles.Any(xx => x.VehicleId == xx.VehicleId), x => Array.Find(_vehicles.ToArray(), xx => x.VehicleId == xx.VehicleId)
    }

    public void Remove(IEnumerable<V.Vehicle> entities)
    {
        foreach(var entity in entities)
        {
            _vehicles.Remove(entity);
        }
    }

    public void Add(IEnumerable<Person> entities)
    {
        throw new NotImplementedException();
    }

    public void Update(IEnumerable<Person> entities)
    {
        throw new NotImplementedException();
    }

    public void Remove(IEnumerable<Person> entities)
    {
        throw new NotImplementedException();
    }

    public void Add(IEnumerable<VehicleInformation> entities)
    {
        throw new NotImplementedException();
    }

    public void Update(IEnumerable<VehicleInformation> entities)
    {
        throw new NotImplementedException();
    }

    public void Remove(IEnumerable<VehicleInformation> entities)
    {
        throw new NotImplementedException();
    }

    public void Add(IEnumerable<LicenseType> entities)
    {
        throw new NotImplementedException();
    }

    public void Update(IEnumerable<LicenseType> entities)
    {
        throw new NotImplementedException();
    }

    public void Remove(IEnumerable<LicenseType> entities)
    {
        throw new NotImplementedException();
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

    //private static void UpdateCollection<T>(HashSet<T> collection, IEnumerable<T> entities, Expression<Func<T, bool>> predicateAdd, Expression<Func<T, bool>> predicateRemove, Predicate<T> removeWhere)
    //{
    //    foreach (var entity in entities.AsQueryable().Where(predicateAdd))
    //    {
    //        collection.Add(entity);
    //    }
    //    foreach (var entity in entities.AsQueryable().Where(predicateRemove))
    //    {
    //        collection.RemoveWhere(removeWhere);
    //        collection.Add(entity);
    //    }
    //}
}
