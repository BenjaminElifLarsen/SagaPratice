using Common.BinaryFlags;
using Common.SpecificationPattern;
using Common.SpecificationPattern.Composite.Extensions;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Errrors;
using PersonDomain.DL.Validation.GenderSpecifications;

namespace PersonDomain.DL.Validation;
internal sealed class GenderValidator : IValidate
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
