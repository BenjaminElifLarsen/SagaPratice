using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Validation.PersonSpecifications;
internal class IsPersonBirthSat : ISpecification<Person>, ISpecification<HirePersonFromUser>
{
    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
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
