using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.People.Validation;
using VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.People;
internal class License
{
    private int _licenseId;
    private Person _owner; //ORMs like EF Core would use and set this variable
    private IdReference _type;
    private DateTime _arquired;
    private DateTime? _lastRenewed;
    private bool _expired;

    public int LicenseId { get => _licenseId; private set => _licenseId = value; }
    public DateTime Arquired { get => _arquired; private set => _arquired = value; }
    public DateTime? LastRenewed { get => _lastRenewed; private set => _lastRenewed = value; }
    public bool Expired { get => _expired; private set => _expired = value; }
    public IdReference Type { get => _type; private set => _type = value; }

    private License()
    {

    }

    internal License(IdReference type, DateTime arquired)
    {
        _licenseId = new Random(int.MaxValue).Next(); //mock up id generation.
        _type = type;
        _arquired = arquired;
        _lastRenewed = null;
        _expired = false;
    }

    public int UpdateArquired(DateTime arquired, LicenseValidationData licenseTypeAgeValidation)
    {
        if(!new IsLicenseArquiredValid(licenseTypeAgeValidation).IsSatisfiedBy(arquired))
        {
            return (int)LicenseErrors.InvalidArquired;
        }
        _arquired = arquired;
        return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool CheckIfExpired()
    { //check if last renewed is set or not, subtract current date against arquired/lastrenewed date, get years and compare to RenewPeriodInYears in licenseType
        throw new NotImplementedException();
    }

    public bool CheckIfAboutToExpire() //the handler that call this one should tigger an event that could be used to create an email, sms, or similar
    {//check if there is less than half a year left
        throw new NotImplementedException();
    }

    private DateTime GetCompareDate()
    {
        return _lastRenewed is not null ? _lastRenewed.Value : _arquired;
    }

}
