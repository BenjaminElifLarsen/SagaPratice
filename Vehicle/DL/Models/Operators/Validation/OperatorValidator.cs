using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using Common.SpecificationPattern.Composite.Extensions;
using l = VehicleDomain.DL.Models.Operators.CQRS.Commands.License;
using lv = VehicleDomain.DL.Models.Operators.Validation.PersonCreationLicenseValidationData.LicenseValidationData;
using Common.Other;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
using VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;
using VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.Operators.Validation;
internal class OperatorValidatorFromSystem : IValidate
{
    private readonly AddOperatorNoLicenseFromSystem _operator;
    public OperatorValidatorFromSystem(AddOperatorNoLicenseFromSystem @operator)
    {
        _operator = @operator;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsOperatorIdSet().IsSatisfiedBy(_operator) ? 0 : OperatorErrors.IdNotSet;
        flag += new IsOperatorOfValidAge().IsSatisfiedBy(_operator) ? 0 : OperatorErrors.InvalidBirth;
        return flag;
    }
}

internal class OperatorValidatorFromUser : IValidate
{
    private readonly byte _maxAge;
    private readonly byte _minAge;
    private readonly AddOperatorWithLicenseFromUser _operator;
    private readonly OperatorValidationData _validationData;
    public OperatorValidatorFromUser(AddOperatorWithLicenseFromUser @operator, OperatorValidationData validationData, byte maxAge, byte minAge)
    {
        _operator = @operator;
        _validationData = validationData;
        _maxAge = maxAge;
        _minAge = minAge;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsOperatorIdSet().IsSatisfiedBy(_operator) ? 0 : OperatorErrors.IdNotSet;
        flag += new IsOperatorOfValidAge().IsSatisfiedBy(_operator) ? 0 : OperatorErrors.InvalidBirth;
        flag += new IsOperatorWithinLicenseAgeRequirement(_validationData).IsSatisfiedBy(_operator) ? 0 : OperatorErrors.InvalidAgeForLicense;
        flag += new IsOperatorToYoung(_minAge).And<AddOperatorWithLicenseFromUser>(new IsOperatorToOld(_maxAge)).IsSatisfiedBy(_operator) ? 0 : OperatorErrors.NotWithinAgeRange;
        return flag;
    }
}

internal class OperatorLicenseValidatorFromUser : IValidate
{
    private readonly l _license;
    private readonly lv _validationData;
    private readonly IEnumerable<LicenseTypeIdValidation> _permittedLicenseTypeIds;

    public OperatorLicenseValidatorFromUser(l license, lv validationData, IEnumerable<LicenseTypeIdValidation> permittedLicenseTypeIds)
    {
        _license = license;
        _validationData = validationData;
        _permittedLicenseTypeIds = permittedLicenseTypeIds;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsLicenseArquiredValid(_validationData).IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidArquired;
        flag += new IsLicenseLicenseTypeSet().And(new IsLicenseLicenseTypeValid(_permittedLicenseTypeIds)).IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidLicenseType;
        return flag;
    }
}