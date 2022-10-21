using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.People.Validation.PersonSpecifications;
using Common.SpecificationPattern.Composite.Extensions;
using VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;
using l = VehicleDomain.DL.CQRS.Commands.License; // Without this one, License would point to the model in the People folder.
using lv = VehicleDomain.DL.Models.People.Validation.PersonCreationLicenseValidationData.LicenseValidationData;
using VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;

namespace VehicleDomain.DL.Models.People.Validation;
internal class PersonValidatorFromSystem : IValidate
{
    private readonly AddPersonNoLicenseFromSystem _person;
    public PersonValidatorFromSystem(AddPersonNoLicenseFromSystem person)
    {
        _person = person;
    }

    public int Validate()
    {
        int flag = 0;
        flag += new IsPersonIdSet().And<AddPersonNoLicenseFromSystem>(new IsPersonIdSet()).IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.IdNotSet;
        flag += new IsPersonOfValidAge().IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.InvalidBirth;
        return flag;
    }
}

internal class PersonValidatorFromUser : IValidate
{
    private readonly byte _maxAge;
    private readonly byte _minAge;
    private readonly AddPersonWithLicenseFromUser _person;
    private readonly PersonValidationData _validationData;
    public PersonValidatorFromUser(AddPersonWithLicenseFromUser person, PersonValidationData validationData, byte maxAge, byte minAge)
    {
        _person = person;
        _validationData = validationData;
        _maxAge = maxAge;
        _minAge = minAge;
    }

    public int Validate()
    {
        int flag = 0;
        flag += new IsPersonIdSet().IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.IdNotSet;
        flag += new IsPersonOfValidAge().IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.InvalidBirth;
        flag += new IsPersonWithinLicenseAgeRequirement(_validationData).IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.InvalidAgeForLicense;
        flag += new IsPersonToYoung(_minAge).And(new IsPersonToOld(_maxAge)).IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.NotWithinAgeRange;
        return flag;
    }
}

internal class PersonLicenseValidatorFromUser : IValidate
{
    private readonly l _license;
    private readonly lv _validationData;
    private readonly IEnumerable<LicenseTypeIdValidation> _permittedLicenseTypeIds;

    public PersonLicenseValidatorFromUser(l license, lv validationData, IEnumerable<LicenseTypeIdValidation> permittedLicenseTypeIds)
    {
        _license = license;
        _validationData = validationData;
        _permittedLicenseTypeIds = permittedLicenseTypeIds;
    }

    public int Validate()
    {
        int flag = 0;
        flag += new IsLicenseArquiredValid(_validationData).IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidArquired;
        flag += new IsLicenseLicenseTypeSet().And(new IsLicenseLicenseTypeValid(_permittedLicenseTypeIds)).IsSatisfiedBy(_license) ? 0 : (int)LicenseErrors.InvalidLicenseType;
        return flag;
    }
}