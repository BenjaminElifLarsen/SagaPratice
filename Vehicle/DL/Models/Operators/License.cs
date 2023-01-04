using Common.RepositoryPattern;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.Operators.Validation;
using VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.Operators;
internal sealed class License
{
    private int _licenseId;
    private Operator _operator; //ORMs like EF Core would use and set this variable
    private IdReference<int> _type;
    private DateOnly _arquired;
    private DateOnly? _lastRenewed;
    private bool _expired;
    //soft delete, need to deal with license type soft delete, could give either ISoftDelete or ISfotDeleteDate.
    //If going with ISoftDelete, the system needs to validate at n timeperiod if date is or has passed the ISoftDeleteDate over in the specific license type.
    internal int LicenseId { get => _licenseId; private set => _licenseId = value; }
    internal DateOnly Arquired { get => _arquired; private set => _arquired = value; }
    internal DateOnly? LastRenewed { get => _lastRenewed; private set => _lastRenewed = value; }
    internal bool Expired { get => _expired; private set => _expired = value; }
    internal IdReference<int> Type { get => _type; private set => _type = value; }

    private License()
    {

    }

    internal License(IdReference<int> type, DateOnly arquired)
    {
        _licenseId = RandomValue.GetValue;
        _type = type;
        _arquired = arquired;
        _lastRenewed = null;
        _expired = false;
    }

    internal int UpdateArquired(DateTime arquired, LicenseValidationData licenseTypeAgeValidation)
    {
        if (!new IsLicenseArquiredValid(licenseTypeAgeValidation).IsSatisfiedBy(arquired)) //not happy with having the validatio data here, bad for purity
        {
            return (int)LicenseErrors.InvalidArquired;
        }
        _arquired = new(arquired.Year, arquired.Month, arquired.Day);
        return 0;
    }

    internal bool Review(DateOnly date)
    {
        if (date <= _arquired)
            return false;
        _lastRenewed = date; //need to check if date also is before _lastRenewed
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal bool CheckIfExpired()
    { //check if last renewed is set or not, subtract current date against arquired/lastrenewed date, get years and compare to RenewPeriodInYears in licenseType
        throw new NotImplementedException();
    }

    internal bool CheckIfAboutToExpire() //the handler that call this one should tigger an event that could be used to create an email, sms, or similar
    {//check if there is less than half a year left
        throw new NotImplementedException();
    }

    private DateOnly GetCompareDate()
    {
        return _lastRenewed is not null ? _lastRenewed.Value : _arquired;
    }

    internal bool ValidateAgeRequirement(byte age, byte ageRequirement)
    {
        return age >= ageRequirement;
    }

    internal bool ValidateRenewPeriod(byte renewPeriod)
    {
        var now = DateTime.Now; //consider if there is a good way to make this DRY
        var lastRenew = GetCompareDate();
        var sinceLastRenew = now.Year - lastRenew.Year - 1 +
        (now.Month > lastRenew.Month ||
        now.Month == lastRenew.Month && now.Day >= lastRenew.Day ? 1 : 0);
        bool timePassed = sinceLastRenew < renewPeriod;
        if (!timePassed)
        {
            _expired = true;
        }
        return timePassed;
    }
}
