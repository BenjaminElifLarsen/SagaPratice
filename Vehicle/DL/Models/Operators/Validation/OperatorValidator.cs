using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using Common.SpecificationPattern.Composite.Extensions;
using l = VehicleDomain.DL.Models.Operators.CQRS.Commands.License; // Without this one, License would point to the model in the People folder.
using lv = VehicleDomain.DL.Models.Operators.Validation.PersonCreationLicenseValidationData.LicenseValidationData;
using VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;
using Common.Other;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
using VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;

namespace VehicleDomain.DL.Models.Operators.Validation;
internal class OperatorValidatorFromSystem : IValidate
{
    private readonly AddPersonNoLicenseFromSystem _operator;
    public OperatorValidatorFromSystem(AddPersonNoLicenseFromSystem @operator)
    {
        _operator = @operator;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        if (new IsOperatorIdSet().And<AddPersonNoLicenseFromSystem>(new IsOperatorIdSet()).IsSatisfiedBy(_operator))
            flag.AddFlag((int)OperatorErrors.IdNotSet);
        if (new IsOperatorOfValidAge().IsSatisfiedBy(_operator))
            flag.AddFlag((int)OperatorErrors.InvalidBirth);
        return flag;
    }
}

internal class OperatorValidatorFromUser : IValidate
{
    private readonly byte _maxAge;
    private readonly byte _minAge;
    private readonly AddPersonWithLicenseFromUser _operator;
    private readonly OperatorValidationData _validationData;
    public OperatorValidatorFromUser(AddPersonWithLicenseFromUser @operator, OperatorValidationData validationData, byte maxAge, byte minAge)
    {
        _operator = @operator;
        _validationData = validationData;
        _maxAge = maxAge;
        _minAge = minAge;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        if (new IsOperatorIdSet().IsSatisfiedBy(_operator))
            flag.AddFlag((int)OperatorErrors.IdNotSet);
        if (new IsOperatorOfValidAge().IsSatisfiedBy(_operator))
            flag.AddFlag((int)OperatorErrors.InvalidBirth);
        if (new IsOperatorWithinLicenseAgeRequirement(_validationData).IsSatisfiedBy(_operator))
            flag.AddFlag((int)OperatorErrors.InvalidAgeForLicense);
        if (new IsOperatorToYoung(_minAge).And(new IsOperatorToOld(_maxAge)).IsSatisfiedBy(_operator))
            flag.AddFlag((int)OperatorErrors.NotWithinAgeRange);
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