using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation;
internal class LicenseTypeErrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if(binaryFlag == LicenseTypeErrors.InvalidAgeRequirement)
        {
            errors.Add($"License type age requirement is invalid.");
        }
        if (binaryFlag == LicenseTypeErrors.InvalidType)
        {
            errors.Add($"License type type is invalid.");
        }
        if (binaryFlag == LicenseTypeErrors.InvalidRenewPeriod)
        {
            errors.Add($"License type renew period is invalid.");
        }
        return errors;
    }
}
