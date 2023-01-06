using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.DL.Validation.GenderSpecifications;

internal sealed class IsGenderVerbObjectSat : ISpecification<RecogniseGender>
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