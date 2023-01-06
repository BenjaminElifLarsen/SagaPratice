using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.DL.Validation.GenderSpecifications;
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
