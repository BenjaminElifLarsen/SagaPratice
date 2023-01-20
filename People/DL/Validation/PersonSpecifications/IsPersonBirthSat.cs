using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Validation.PersonSpecifications;
internal sealed class IsPersonBirthSat : ISpecification<Person>, ISpecification<HirePersonFromUser>, ISpecification<ChangePersonalInformationFromUser>
{
    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }

    public bool IsSatisfiedBy(ChangePersonalInformationFromUser candidate)
    {
        return candidate.Birth is null || IsSatisfiedBy(candidate.Birth.Birth);
    }

    private bool IsSatisfiedBy(DateTime candidate)
    {
        return IsSatisfiedBy(new DateOnly(candidate.Year, candidate.Month, candidate.Day));
    }

    private bool IsSatisfiedBy(DateOnly candidate)
    {
        return candidate != default;
    }
}
