using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.CQRS.Queries.ReadModels;

namespace PeopleDomain.DL.Validation.GenderSpecifications;
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
