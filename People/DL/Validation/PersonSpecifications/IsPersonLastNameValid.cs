using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Validation.PersonSpecifications;
internal sealed class IsPersonLastNameValid : ISpecification<Person>, ISpecification<HirePersonFromUser>, ISpecification<ChangePersonalInformationFromUser>
{
    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.LastName);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }

    public bool IsSatisfiedBy(ChangePersonalInformationFromUser candidate)
    {
        return candidate.LastName is null || IsSatisfiedBy(candidate.LastName.LastName);
    }

    private static bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
