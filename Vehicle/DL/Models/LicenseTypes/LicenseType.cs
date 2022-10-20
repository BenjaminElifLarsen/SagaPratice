using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseType : IAggregateRoot
{
    private int _licenseTypeId;
    private string _type;
    private byte _renewPeriodInYears;
    private byte _ageRequirementInYears;

    public int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }
    public string Type { get => _type; private set => _type = value; }
    public byte RenewPeriodInYears { get => _renewPeriodInYears; private set => _renewPeriodInYears = value; }
    public byte AgeRequirementInYears { get => _ageRequirementInYears; private set => _ageRequirementInYears = value; }

    private LicenseType()
    {

    }

    internal LicenseType(int licenseTypeId, string type, byte renewPeriodInYears, byte ageRequirementInYears)
    {
        _licenseTypeId = licenseTypeId;
        _type = type;
        _renewPeriodInYears = renewPeriodInYears;
        _ageRequirementInYears = ageRequirementInYears;
    }
}
