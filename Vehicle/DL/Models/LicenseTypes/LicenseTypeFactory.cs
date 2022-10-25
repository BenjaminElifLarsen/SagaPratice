using Common.Other;
using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Validation;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal class LicenseTypeFactory : ILicenseTypeFactory
{
    public Result<LicenseType> CreateLicenseType(EstablishLicenseTypeFromUser licenseType)
    {
        List<string> errors = new();

        BinaryFlag flag = new LicenseTypeValidator(licenseType).Validate();
        if(flag != 0)
        {
            errors.AddRange(LicenseTypeErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<LicenseType>(errors.ToArray());
        }

        LicenseType entity = new(licenseType.Type, licenseType.RenewPeriod, licenseType.AgeRequirement);
        return new SuccessResult<LicenseType>(entity);
    }
}
