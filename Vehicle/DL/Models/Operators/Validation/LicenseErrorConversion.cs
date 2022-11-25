using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using l = VehicleDomain.DL.Models.Operators.CQRS.Commands.License;

namespace VehicleDomain.DL.Models.Operators.Validation;

internal class LicenseErrorConversion : IErrorConversion<l>
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag, l license)
    {
        List<string> errors = new();
        if (binaryFlag == LicenseErrors.LicenseTypeNotSat)
        {
            errors.Add($"License type not sat.");
        }
        if (binaryFlag == LicenseErrors.InvalidLicenseType)
        {
            errors.Add($"License type, {license.LicenseTypeId}, is invalid.");
        }
        if (binaryFlag == LicenseErrors.InvalidArquired)
        {
            errors.Add($"License, {license.LicenseTypeId}, arquired is invalid.");
        }
        if (binaryFlag == LicenseErrors.InvalidLastRenewed)
        {
            errors.Add($"License, {license.LicenseTypeId}, last renew date is invalid.");
        }
        return errors;
    }
}