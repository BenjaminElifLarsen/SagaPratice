using Common.BinaryFlags;
using Common.SpecificationPattern;
using Common.SpecificationPattern.Composite.Extensions;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Validation.GenderSpecifications;

namespace PeopleDomain.DL.Validation;
internal class GenderValidator : IValidate
{
    private readonly RecogniseGender _gender;
    private readonly GenderValidationData _validationData;
    public GenderValidator(RecogniseGender gender, GenderValidationData validationData)
    {
        _gender = gender;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsGenderVerbObjectSat().IsSatisfiedBy(_gender) ? 0 : GenderErrors.InvalidVerbObject;
        flag += new IsGenderVerbSubjectSat().IsSatisfiedBy(_gender) ? 0 : GenderErrors.InvalidVerbSubject;
        flag += new IsGenderVerbObjectNotInUse(_validationData).Or(new IsGenderVerbSubjectNotInUse(_validationData)).IsSatisfiedBy(_gender) ? 0 : GenderErrors.VerbObjectAndSubjectInUse;
        return flag;
    }
}
