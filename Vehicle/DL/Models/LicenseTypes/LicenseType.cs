using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseType : IAggregateRoot, ISoftDeleteDate
{
    private int _licenseTypeId;
    private string _type; //type can only be updated if there is no license that use the entity, need a query that look if any people got license with the specifc license type id
    private byte _renewPeriodInYears; //makes more sense to just use any(x => ...), in the repo, before trying to update, where should validation be done... entity? validator? handler? 
    private byte _ageRequirementInYears;
    private DateTime? _deletedFrom;
    private DateTime _canBeIssuedFrom;

    internal int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }
    internal string Type { get => _type; private set => _type = value; }
    internal byte RenewPeriodInYears { get => _renewPeriodInYears; private set => _renewPeriodInYears = value; }
    internal byte AgeRequirementInYears { get => _ageRequirementInYears; private set => _ageRequirementInYears = value; }
    public DateTime? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }
    public DateTime CanBeIssuedFrom { get => _canBeIssuedFrom; private set => _canBeIssuedFrom = value; } //can only be updated if there is no licenses that use it.

    private LicenseType()
    {

    }

    internal LicenseType( string type, byte renewPeriodInYears, byte ageRequirementInYears)
    {
        _licenseTypeId = new Random(int.MaxValue).Next(); //mock up id generation.
        _type = type;
        _renewPeriodInYears = renewPeriodInYears;
        _ageRequirementInYears = ageRequirementInYears;
    }

    public void Delete(DateTime? dateTime)
    {
        _deletedFrom = dateTime;
    }

}
