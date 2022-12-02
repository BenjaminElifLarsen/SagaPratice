using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.DL.Validation.GenderSpecifications;
internal sealed class IsGenderVerbSubjectSat : ISpecification<RecogniseGender>
{
    public bool IsSatisfiedBy(RecogniseGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbSubject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
