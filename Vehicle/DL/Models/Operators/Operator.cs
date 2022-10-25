using Common.RepositoryPattern;
using Common.SpecificationPattern.Composite.Extensions;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;

namespace VehicleDomain.DL.Models.Operators;

public class Operator : IAggregateRoot
{
    private int _operatorId;
    private DateTime _birth;
    private HashSet<License> _licenses;
    private HashSet<IdReference> _vehicles;

    internal int OperatorId { get => _operatorId; private set => _operatorId = value; }
    internal DateTime Birth { get => _birth; private set => _birth = value; }

    private Operator()
    {

    }

    internal Operator(int operatorId, DateTime birth)
    {
        _operatorId = operatorId;
        _birth = birth;
        _licenses = new HashSet<License>();
        _vehicles = new HashSet<IdReference>();
    }

    internal int UpdateBirth(DateTime birth)
    { //if changing the birth, need to check if all licenses are still valid regarding their age requirements. Could also do the young/old specification
        if (!new IsOperatorOfValidAge().IsSatisfiedBy(birth)) //if a license is invalid, what to do? Revoke the license or fail the update and inform the caller of the problem?
        {
            return (int)OperatorErrors.InvalidBirth;
        }
        if(!new IsOperatorToYoung(10).And<DateTime>(new IsOperatorToOld(80)).IsSatisfiedBy(birth)){
            return (int)OperatorErrors.NotWithinAgeRange;
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
