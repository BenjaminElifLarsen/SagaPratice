using Common.Other.Converters;
using Common.RepositoryPattern;
using Common.SpecificationPattern.Composite.Extensions;
using System.Text.Json.Serialization;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;

namespace VehicleDomain.DL.Models.Operators;

public class Operator : IAggregateRoot, ISoftDelete
{
    private int _operatorId;
    private DateOnly _birth;
    private HashSet<License> _licenses;
    private HashSet<IdReference> _vehicles;
    private bool _deleted; 

    internal int OperatorId { get => _operatorId; private set => _operatorId = value; }

    internal DateOnly Birth { get => _birth; private set => _birth = value; }

    internal IEnumerable<License> Licenses => _licenses;
    internal IEnumerable<IdReference> Vehicles => _vehicles;

    public bool Deleted { get => _deleted; private set => _deleted = value; }

    private Operator()
    {

    }

    internal Operator(int operatorId, DateOnly birth)
    {
        _operatorId = operatorId;
        _birth = birth;
        _licenses = new();
        _vehicles = new();
    }

    internal int UpdateBirth(DateOnly birth)
    { //if changing the birth, need to check if all licenses are still valid regarding their age requirements. Could also do the young/old specification
        if (!new IsOperatorOfValidAge().IsSatisfiedBy(birth)) //if a license is invalid, what to do? Revoke the license or fail the update and inform the caller of the problem?
        {
            return (int)OperatorErrors.InvalidBirth;
        }
        if(!new IsOperatorToYoung(10).And<DateOnly>(new IsOperatorToOld(80)).IsSatisfiedBy(birth)){
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

    internal bool AddLicense(IdReference type, DateOnly arquired)
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

    public void Delete()
    {
        throw new NotImplementedException();
    }
}
