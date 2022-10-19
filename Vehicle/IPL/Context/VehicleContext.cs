using BaseRepository;
using Vehicle.DL.Models;

namespace Vehicle.IPL.Context;
internal class MockVehicleContext : IContext<DL.Models.Vehicle>, IContext<LicenseType>, IContext<VehicleInformation>, IContext<Person>
{
    private HashSet<DL.Models.Vehicle> _vehicles;
    public HashSet<DL.Models.Vehicle> Vehicles => _vehicles;

    private HashSet<LicenseType> _licenseTypes;
    public HashSet<LicenseType> LicenseTypes => _licenseTypes;

    private HashSet<VehicleInformation> _vehicleInformation;
    public HashSet<VehicleInformation> VehicleInformation => _vehicleInformation;

    private HashSet<Person> _people;
    public HashSet<Person> People => _people;

    public MockVehicleContext()
    {
        _vehicles = new();
        _people = new();
        _licenseTypes = new();
        _vehicleInformation = new();
    }

    public void Add(IEnumerable<DL.Models.Vehicle> entities)
    {
        if(_vehicles.Any(x => entities.Any(xx => x.VehicleId == xx.VehicleId)))
        {
            throw new Exception("Entity already present.");
        }
        foreach(var entity in entities)
        {
            _vehicles.Add(entity);
        }
    }

    public void Update(IEnumerable<DL.Models.Vehicle> entities)
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
    }

    public void Remove(IEnumerable<DL.Models.Vehicle> entities)
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
}
