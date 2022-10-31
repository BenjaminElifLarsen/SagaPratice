using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.CQRS.Queries.ReadModels;

namespace PeopleDomain.DL.Validation.GenderSpecifications;
internal class IsGenderVerbObjectNotInUse : ISpecification<PermitGender>
{
    private IEnumerable<GenderVerb> _genderVerbs;
    public IsGenderVerbObjectNotInUse(GenderValidationData validationData)
    {
        _genderVerbs = validationData.GenderVerbs;
    }

    public bool IsSatisfiedBy(PermitGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbObject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !_genderVerbs.Any(x => x.Object == candidate);
    }
}
