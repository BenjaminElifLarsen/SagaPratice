using Common.BinaryFlags;
using Common.SpecificationPattern;
using Common.SpecificationPattern.Composite.Extensions;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Validation.PersonSpecifications;

namespace PeopleDomain.DL.Validation;
internal sealed class PersonHireValidator : IValidate
{
    private readonly HirePersonFromUser _person;
    private readonly PersonValidationData _validationData;

    public PersonHireValidator(HirePersonFromUser person, PersonValidationData validationData)
    {
        _person = person;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsPersonBirthSat().And<HirePersonFromUser>(new IsPersonBirthNotToLate()).IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidBirth;
        flag += new IsPersonFirstNameValid().IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidFirstName;
        flag += new IsPersonLastNameValid().IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidLastName;
        flag += new IsPersonGenderSat().And<HirePersonFromUser>(new IsPersonGenderValid(_validationData)).IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidGender;
        return flag;
    }
}

internal class PersonChangePersonalInformationValidator : IValidate
{
    private readonly ChangePersonalInformationFromUser _person;
    private readonly PersonValidationData _validationData;

    public PersonChangePersonalInformationValidator(ChangePersonalInformationFromUser person, PersonValidationData validationData)
    {
        _person = person;
        _validationData = validationData;            
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsPersonBirthSat().And<ChangePersonalInformationFromUser>(new IsPersonBirthNotToLate()).IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidBirth;
        flag += new IsPersonFirstNameValid().IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidFirstName;
        flag += new IsPersonLastNameValid().IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidLastName;
        flag += new IsPersonGenderSat().And<ChangePersonalInformationFromUser>(new IsPersonGenderValid(_validationData)).IsSatisfiedBy(_person) ? 0 : PersonErrors.InvalidGender;
        return flag;
    }
}
