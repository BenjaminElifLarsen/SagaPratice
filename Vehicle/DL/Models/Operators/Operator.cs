using Common.Events.Domain;
using Common.RepositoryPattern;
using Common.SpecificationPattern.Composite.Extensions;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;

namespace VehicleDomain.DL.Models.Operators;

public class Operator : IAggregateRoot, ISoftDelete
{
    private int _operatorId;
    private DateOnly _birth;
    private readonly HashSet<License> _licenses;
    private readonly HashSet<IdReference<int>> _vehicles;
    private bool _deleted;
    private readonly HashSet<IDomainEvent> _events;

    internal int OperatorId { get => _operatorId; private set => _operatorId = value; }

    internal DateOnly Birth { get => _birth; private set => _birth = value; }

    internal IEnumerable<License> Licenses => _licenses;
    internal IEnumerable<IdReference<int>> Vehicles => _vehicles;

    public bool Deleted { get => _deleted; private set => _deleted = value; }

    public IEnumerable<IDomainEvent> Events => _events;

    private Operator()
    {
        _events = new();
    }

    internal Operator(int operatorId, DateOnly birth)
    {
        _operatorId = operatorId;
        _birth = birth;
        _licenses = new();
        _vehicles = new();
        _events = new();
    }

    internal byte CalculateAge()
    {
        throw new NotImplementedException();
    }

    internal int UpdateBirth(DateTime birth)
    { //if changing the birth, need to check if all licenses are still valid regarding their age requirements. Could also do the young/old specification
        if (!new IsOperatorOfValidAge().IsSatisfiedBy(birth)) //if a license is invalid, what to do? Revoke the license or fail the update and inform the caller of the problem?
        {
            return (int)OperatorErrors.InvalidBirth;
        } //hard coded ages, not the best
        if(!new IsOperatorToYoung(10).And<DateTime>(new IsOperatorToOld(80)).IsSatisfiedBy(birth)){
            return (int)OperatorErrors.NotWithinAgeRange;
        }
        _birth = new(birth.Year, birth.Month, birth.Day);
        return 0;
    }

    internal bool AddVehicle(IdReference<int> vehicle)
    { //check if they got the correct license first
        return _vehicles.Add(vehicle);
    }

    internal bool RemoveVehicle(IdReference<int> vehicle)
    {
        return _vehicles.Remove(vehicle);
    }

    internal IdReference<int> GetVehicle(int id)
    {
        return _vehicles.FirstOrDefault(x => x.Id == id);
    }

    internal bool AddLicense(IdReference<int> type, DateTime arquired)
    {
        if (_licenses.Any(x => x.Type.Id == type.Id))
            return false;
        return _licenses.Add(new(type, new(arquired.Year, arquired.Month, arquired.Day)));
    }

    internal bool RemoveLicense(License license)
    {
        return _licenses.Remove(license);
    }

    internal License GetLicenseViaLicenseType(int typeId)
    {
        return _licenses.FirstOrDefault(x => x.Type.Id == typeId);
    }

    internal IEnumerable<License> GetValidLincenses()
    {
        return _licenses.Where(x => x.Expired == false);
    }

    public bool RenewLicense(int LicenseTypeId, DateOnly renewDate)
    {
        var license = GetLicenseViaLicenseType(LicenseTypeId);
        if (license is null)
            return false;
        return license.Review(renewDate);
    }

    public void Delete()
    {
        _deleted = true;
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        if (_operatorId == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        if (_operatorId == eventItem.AggregateId) //should cause an expection if this fails
            _events.Remove(eventItem);
    }
}
