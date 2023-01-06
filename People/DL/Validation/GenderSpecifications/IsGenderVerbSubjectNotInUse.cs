using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.CQRS.Queries.ReadModels;

namespace PersonDomain.DL.Validation.GenderSpecifications;
internal sealed class IsGenderVerbSubjectNotInUse : ISpecification<RecogniseGender>
{
    private IEnumerable<GenderVerbValidation> _genderVerbs;
    public IsGenderVerbSubjectNotInUse(GenderValidationData validationData)
    {
        _genderVerbs = validationData.GenderVerbs;
    }

    public bool IsSatisfiedBy(RecogniseGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbSubject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !_genderVerbs.Any(x => x.Subject == candidate);
    }
}
