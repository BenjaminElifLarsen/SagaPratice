using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.People.Validation.PersonSpecifications;
using Common.SpecificationPattern.Composite.Extensions;

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
    private readonly LicenseValidationData _validationData;
    public PersonValidatorFromUser(AddPersonWithLicenseFromUser person, LicenseValidationData validationData, byte maxAge, byte minAge)
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
        flag += new IsPersonWIthinLicenseAgeRequirement(_validationData.LicenseTypes.Select(x => x.AgeRequirementInYears)).IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.InvalidAgeForLicense;
        flag += new IsPersonToYoung(_minAge).And(new IsPersonToOld(_maxAge)).IsSatisfiedBy(_person) ? 0 : (int)PersonErrors.NotWithinAgeRange;
        return flag;
    }
}