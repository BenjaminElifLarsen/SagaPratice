using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.DL.Validation.GenderSpecifications;

internal class IsGenderVerbObjectSat : ISpecification<RecogniseGender>
{
    public bool IsSatisfiedBy(RecogniseGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbObject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}