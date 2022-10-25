using Common.RepositoryPattern;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.People.Validation.PersonSpecifications;

namespace VehicleDomain.DL.Models.People;

public class Person : IAggregateRoot
{
    private int _personId;
    private DateTime _birth;
    private HashSet<License> _licenses;
    private HashSet<IdReference> _vehicles;

    internal int PersonId { get => _personId; private set => _personId = value; }
    internal DateTime Birth { get => _birth; private set => _birth = value; }

    private Person()
    {

    }

    internal Person(int personId, DateTime birth)
    {
        _personId = personId;
        _birth = birth;
        _licenses = new HashSet<License>();
        _vehicles = new HashSet<IdReference>();
    }

    internal int UpdateBirth(DateTime birth)
    { //if changing the birth, need to check if all licenses are still valid regarding their age requirements. Could also do the young/old specification
        if (!new IsPersonOfValidAge().IsSatisfiedBy(birth))
        {
            return (int)PersonErrors.InvalidBirth;
        }
        _birth = birth;
        return 0;
    }

    internal bool AddVehicle(IdReference vehicle)
    {
        return _vehicles.Add(vehicle);
    }

    internal bool RemoveVehicle(IdReference vehicle)
    {
        return _vehicles.Remove(vehicle);
    }

    internal IdReference GetVehicle(int id)
    {
        return _vehicles.FirstOrDefault(x => x.Id == id);
    }

    internal bool AddLicense(IdReference type, DateTime arquired)
    {
        if (_licenses.Any(x => x.Type.Id == type.Id))
            return false;
        return _licenses.Add(new(type, arquired));
    }

    internal bool RemoveLicense(License license)
    {
        return _licenses.Remove(license);
    }

    internal License GetLicense(int typeId)
    {
        return _licenses.FirstOrDefault(x => x.Type.Id == typeId);
    }

    internal IEnumerable<License> GetValidLincenses()
    {
        return _licenses.Where(x => x.Expired == false);
    }

}
