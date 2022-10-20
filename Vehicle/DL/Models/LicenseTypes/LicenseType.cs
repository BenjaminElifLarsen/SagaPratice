using Common.RepositoryPattern;

namespace Vehicle.DL.Models.LicenseTypes;
internal class LicenseType : IAggregateRoot
{
    private int _licenseTypeId;
    private string _type;
    private byte _renewPeriodInYears;

    public int LicenseTypeId { get => _licenseTypeId; private set => _licenseTypeId = value; }

    private LicenseType()
    {

    }

    internal LicenseType(int licenseTypeId, string type, byte renewPeriodInYears)
    {
        _licenseTypeId = licenseTypeId;
        _type = type;
        _renewPeriodInYears = renewPeriodInYears;
    }
}
