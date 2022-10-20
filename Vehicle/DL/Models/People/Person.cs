using Common.RepositoryPattern;

namespace Vehicle.DL.Models.People;

internal class Person : IAggregateRoot
{
    private int _personId;
    private DateTime _birth;
    private HashSet<License> _license;
    private HashSet<IdReference> _vehicles;

    public int PersonId { get => _personId; private set => _personId = value; }
    public DateTime Birth { get => _birth; private set => _birth = value; }

    public Person(int personId, DateTime birth)
    {
        _personId = personId;
        _birth = birth;
        _license = new HashSet<License>();
        _vehicles = new HashSet<IdReference>();
    }

    public void UpdateBirth(DateTime birth)
    {
        _birth = birth;
    }

    public bool AddVehicle(IdReference vehicle)
    {
        return _vehicles.Add(vehicle);
    }

    public bool RemoveVehicle(IdReference vehicle)
    {
        return _vehicles.Remove(vehicle);
    }

    public IdReference GetVehicle(int id)
    {
        return _vehicles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddLicense(IdReference type, DateTime arquired)
    {
        return _license.Add(new(type, arquired));
    }

    public bool RemoveLicense(License license)
    {
        return _license.Remove(license);
    }

    public License GetLicense(int id)
    {
        return _license.FirstOrDefault(x => x.LicenseId == id);
    }

    public IEnumerable<License> GetValidLincenses()
    {
        return _license.Where(x => x.Expired == false);
    }

}
