using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.DL.Validation.GenderSpecifications;
internal class IsGenderVerbSubjectSat : ISpecification<PermitGender>
{
    public bool IsSatisfiedBy(PermitGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbSubject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
