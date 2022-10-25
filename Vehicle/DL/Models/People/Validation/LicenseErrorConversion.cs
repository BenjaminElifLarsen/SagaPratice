using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using l = VehicleDomain.DL.Models.People.CQRS.Commands.License;

namespace VehicleDomain.DL.Models.People.Validation;

internal class LicenseErrorConversion : IErrorConversion<l>
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag, l license)
    {
        List<string> errors = new();
        if (binaryFlag.IsFlagPresent((int)LicenseErrors.LicenseTypeNotSat))
        {
            errors.Add($"License type not sat.");
        }
        if (binaryFlag.IsFlagPresent((int)LicenseErrors.InvalidLicenseType))
        {
            errors.Add($"License type, {license.LicenseTypeId}, is invalid.");
        }
        if (binaryFlag.IsFlagPresent((int)LicenseErrors.InvalidArquired))
        {
            errors.Add($"License, {license.LicenseTypeId}, arquired is invalid.");
        }
        if (binaryFlag.IsFlagPresent((int)LicenseErrors.InvalidLastRenewed))
        {
            errors.Add($"License, {license.LicenseTypeId}, last renew date is invalid.");
        }
        return errors;
    }
}