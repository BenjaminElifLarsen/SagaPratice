using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Validation.PersonSpecifications;
internal class IsPersonBirthNotToLate : ISpecification<Person>, ISpecification<HirePersonFromUser>, ISpecification<ChangePersonalInformationFromUser>
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
        return candidate.Brith is null || IsSatisfiedBy(candidate.Brith.Birth);
    }

    private bool IsSatisfiedBy(DateTime candidate)
    {
        return IsSatisfiedBy(new DateOnly(candidate.Year, candidate.Month, candidate.Day));
    }

    private bool IsSatisfiedBy(DateOnly candidate)
    {
        var now = DateTime.Now;
        return candidate < new DateOnly(now.Year, now.Month, now.Day);
    }
}
