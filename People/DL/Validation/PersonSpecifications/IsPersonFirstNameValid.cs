using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Validation.PersonSpecifications;
internal sealed class IsPersonFirstNameValid : ISpecification<Person>, ISpecification<HirePersonFromUser>, ISpecification<ChangePersonalInformationFromUser>
{
    public bool IsSatisfiedBy(ChangePersonalInformationFromUser candidate)
    {
        return candidate.FirstName is null || IsSatisfiedBy(candidate.FirstName.FirstName);
    }

    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.FirstName);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        return IsSatisfiedBy(candidate.FirstName);
    }

    private static bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
