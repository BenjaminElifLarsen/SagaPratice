using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.CQRS.Queries.ReadModels;

namespace PersonDomain.DL.Validation.GenderSpecifications;
internal sealed class IsGenderVerbObjectNotInUse : ISpecification<RecogniseGender>
{
    private IEnumerable<GenderVerbValidation> _genderVerbs;
    public IsGenderVerbObjectNotInUse(GenderValidationData validationData)
    {
        _genderVerbs = validationData.GenderVerbs;
    }

    public bool IsSatisfiedBy(RecogniseGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbObject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !_genderVerbs.Any(x => x.Object == candidate);
    }
}
