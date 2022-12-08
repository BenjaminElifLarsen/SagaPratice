using Common.Events.Domain;
using Common.RepositoryPattern;

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
        var now = DateTime.Now;
        var personAge = now.Year - _birth.Year - 1 +
        (now.Month > _birth.Month ||
        now.Month == _birth.Month && now.Day >= _birth.Day ? 1 : 0);
        return (byte)personAge;
    }

    internal void UpdateBirth(DateTime birth)
    { 
        _birth = new(birth.Year, birth.Month, birth.Day);
    }

    internal bool AddVehicle(IdReference<int> vehicle)
    {
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ageRequirement"></param>
    /// <param name="licenseTypeId"></param>
    /// <returns>Null if the license was not found, true if old enough else false.</returns>
    public bool? ValidateLicenseAgeRequirementIsFulfilled(byte ageRequirement, int licenseTypeId)
    {
        var license = GetLicenseViaLicenseType(licenseTypeId);
        if (license is null) 
        {
            return null;
        }
        return license.ValidateAgeRequirement(CalculateAge(), ageRequirement);
    }

    public bool? ValidateLicenseRenewPeriodIsFulfilled(byte renewPeriod, int licenseTypeId)
    {
        var license = GetLicenseViaLicenseType(licenseTypeId);
        if(license is null)
        {
            return null;
        }
        return license.ValidateRenewPeriod(renewPeriod);
    }
}
