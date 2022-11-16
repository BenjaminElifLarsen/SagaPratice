using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
public class LicenseType : IAggregateRoot, ISoftDeleteDate
{
    private int _licenseTypeId;
    private string _type; //type can only be updated if there is no license that use the entity, need a query that look if any people got license with the specifc license type id
    private byte _renewPeriodInYears; //makes more sense to just use any(x => ...), in the repo, before trying to update, where should validation be done... entity? validator? handler? 
    private byte _ageRequirementInYears; //or would it make more sense that an incorrect, but not invalid, entity would be removed from the system and a new inserted?
    private DateOnly? _deletedFrom;
    private DateOnly _canBeIssuedFrom; //need to be put into ctor and validation, allow update as long time current date is not same or later as its value.
    private readonly HashSet<IdReference<int>> _vehicleInformations;
    //cannot contain a collection of licenses, since License is not an aggregate root, could hold a collection of operators who got the required license.
    private HashSet<IDomainEvent> _events;

    internal int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }
    internal string Type { get => _type; private set => _type = value; }
    internal byte RenewPeriodInYears { get => _renewPeriodInYears; private set => _renewPeriodInYears = value; }
    internal byte AgeRequirementInYears { get => _ageRequirementInYears; private set => _ageRequirementInYears = value; }
    public DateOnly? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }
    public DateOnly CanBeIssuedFrom { get => _canBeIssuedFrom; private set => _canBeIssuedFrom = value; } //can only be updated if there is no licenses that use it.
    public IEnumerable<IdReference<int>> VehicleInformations => _vehicleInformations;

    public IEnumerable<IDomainEvent> Events => _events;

    private LicenseType()
    {
        _events = new();
    }

    internal LicenseType(string type, byte renewPeriodInYears, byte ageRequirementInYears)
    {
        _licenseTypeId = RandomValue.GetValue;
        _type = type;
        _renewPeriodInYears = renewPeriodInYears;
        _ageRequirementInYears = ageRequirementInYears;
        _vehicleInformations = new();
        _events = new();
    }

    public void Delete(DateOnly? dateTime)
    {
        _deletedFrom = dateTime;
    }

    public bool AddVehicleInformation(IdReference<int> vehicleInformation)
    {
        return _vehicleInformations.Add(vehicleInformation);
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        if (_licenseTypeId == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        if (_licenseTypeId == eventItem.AggregateId) //should cause an expection if this fails
            _events.Remove(eventItem);
    }
}
