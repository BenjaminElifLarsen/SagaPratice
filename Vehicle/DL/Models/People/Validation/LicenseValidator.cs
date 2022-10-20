using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.People.Validation;
internal class LicenseValidator : IValidate
{
    private readonly AddLicenseToPerson _license;
    public LicenseValidator(AddLicenseToPerson addLicenseToPerson)
    {
        _license = addLicenseToPerson;
    }

    public int Validate()
    {
        int flag = 0;
        flag += new IsLicenseLicenseTypeSet().IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.LicenseTypeNotSat;
        flag += new IsLicenseArquiredValid().IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidArquired;
        return flag;
    }
}
