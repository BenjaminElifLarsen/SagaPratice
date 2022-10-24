using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseType : IAggregateRoot
{
    private int _licenseTypeId;
    private string _type;
    private byte _renewPeriodInYears;
    private byte _ageRequirementInYears;

    internal int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }
    internal string Type { get => _type; private set => _type = value; }
    internal byte RenewPeriodInYears { get => _renewPeriodInYears; private set => _renewPeriodInYears = value; }
    internal byte AgeRequirementInYears { get => _ageRequirementInYears; private set => _ageRequirementInYears = value; }

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

}
