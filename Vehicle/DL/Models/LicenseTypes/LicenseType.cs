using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseType : IAggregateRoot, ISoftDeleteDate
{
    private int _licenseTypeId;
    private string _type; //type can only be updated if there is no license that use the entity, need a query that look if any people got license with the specifc license type id
    private byte _renewPeriodInYears; //makes more sense to just use any(x => ...), in the repo, before trying to update, where should validation be done... entity? validator? handler? 
    private byte _ageRequirementInYears; //or would it make more sense that an incorrect, but not invalid, entity would be removed from the system and a new inserted?
    private DateOnly? _deletedFrom;
    private DateTime _canBeIssuedFrom;
    private readonly HashSet<IdReference> _licenses;
    private readonly HashSet<IdReference> _vehicleInformations;

    internal int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }
    internal string Type { get => _type; private set => _type = value; }
    internal byte RenewPeriodInYears { get => _renewPeriodInYears; private set => _renewPeriodInYears = value; }
    internal byte AgeRequirementInYears { get => _ageRequirementInYears; private set => _ageRequirementInYears = value; }
    public DateOnly? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }
    public DateTime CanBeIssuedFrom { get => _canBeIssuedFrom; private set => _canBeIssuedFrom = value; } //can only be updated if there is no licenses that use it.
    public IEnumerable<IdReference> Licenses => _licenses;
    public IEnumerable<IdReference> VehicleInformations => _vehicleInformations;

    private LicenseType()
    {

    }

    internal LicenseType( string type, byte renewPeriodInYears, byte ageRequirementInYears)
    {
        _licenseTypeId = RandomValue.GetValue;
        _type = type;
        _renewPeriodInYears = renewPeriodInYears;
        _ageRequirementInYears = ageRequirementInYears;
        _licenses = new();
        _vehicleInformations = new();
    }

    public void Delete(DateOnly? dateTime)
    {
        _deletedFrom = dateTime;
    }

    public bool AddLicense(IdReference license)
    {
        return _licenses.Add(license);
    }

    public bool AddVehicleInformation(IdReference vehicleInformation)
    {
        return _vehicleInformations.Add(vehicleInformation);
    }
}
