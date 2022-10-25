using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.People.CQRS.Commands;
using VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.People.Validation;
internal class LicenseAddToPersonValidator : IValidate
{
    private readonly AddLicenseToPerson _license;
    private readonly LicenseValidationData _validationData;
    public LicenseAddToPersonValidator(AddLicenseToPerson addLicenseToPerson, LicenseValidationData validationData)
    {
        _license = addLicenseToPerson;
        _validationData = validationData;
    }

    public int Validate()
    {
        int flag = 0;
        flag += new IsLicenseLicenseTypeSet().IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.LicenseTypeNotSat;
        flag += new IsLicenseArquiredValid(_validationData).IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidArquired;
        return flag;
    }
}

