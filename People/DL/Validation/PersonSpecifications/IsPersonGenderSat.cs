using Common.SpecificationPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Validation.PersonSpecifications;
internal sealed class IsPersonGenderSat : ISpecification<Person>, ISpecification<HirePersonFromUser>, ISpecification<ChangePersonalInformationFromUser>
{
    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Gender);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }

    public bool IsSatisfiedBy(ChangePersonalInformationFromUser candidate)
    {
        return candidate.Gender is null || IsSatisfiedBy(candidate.Gender.Gender);
    }

    private bool IsSatisfiedBy(Guid candidate)
    {
        return candidate != Guid.Empty;
    }
}
