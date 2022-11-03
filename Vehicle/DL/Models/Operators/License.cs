using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.Operators.Validation;
using VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.Operators;
internal class License
{
    private int _licenseId;
    private Operator _operator; //ORMs like EF Core would use and set this variable
    private IdReference _type;
    private DateOnly _arquired;
    private DateOnly? _lastRenewed;
    private bool _expired;
    //soft delete, need to deal with license type soft delete, could give either ISoftDelete or ISfotDeleteDate.
    //If going with ISoftDelete, the system needs to validate at n timeperiod if date is or has passed the ISoftDeleteDate over in the specific license type.
    public int LicenseId { get => _licenseId; private set => _licenseId = value; }
    public DateOnly Arquired { get => _arquired; private set => _arquired = value; }
    public DateOnly? LastRenewed { get => _lastRenewed; private set => _lastRenewed = value; }
    public bool Expired { get => _expired; private set => _expired = value; }
    public IdReference Type { get => _type; private set => _type = value; }

    private License()
    {

    }

    internal License(IdReference type, DateOnly arquired)
    {
        _licenseId = RandomValue.GetValue;
        _type = type;
        _arquired = arquired;
        _lastRenewed = null;
        _expired = false;
    }

    public int UpdateArquired(DateTime arquired, LicenseValidationData licenseTypeAgeValidation)
    {
        if (!new IsLicenseArquiredValid(licenseTypeAgeValidation).IsSatisfiedBy(arquired)) //not happy with having the validatio data here, bad for purity
        {
            return (int)LicenseErrors.InvalidArquired;
        }
        _arquired = new(arquired.Year, arquired.Month, arquired.Day);
        return 0;
    }

    public bool Review(DateOnly date)
    {
        if (date <= _arquired)
            return false;
        _lastRenewed = date;
        return true;
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

    private DateOnly GetCompareDate()
    {
        return _lastRenewed is not null ? _lastRenewed.Value : _arquired;
    }

}
