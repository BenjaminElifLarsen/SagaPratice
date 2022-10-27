using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.Operators.Validation;
internal class LicenseAddToPersonValidator : IValidate
{
    private readonly AddLicenseToOperator _license;
    private readonly LicenseValidationData _validationData;
    public LicenseAddToPersonValidator(AddLicenseToOperator addLicenseToPerson, LicenseValidationData validationData)
    {
        _license = addLicenseToPerson;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = 0;
        flag += new IsLicenseLicenseTypeSet().IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.LicenseTypeNotSat;
        flag += new IsLicenseArquiredValid(_validationData).IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidArquired;
        return flag;
    }
}

