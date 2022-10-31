using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Validation.PersonSpecifications;
internal class IsPersonGenderSat : ISpecification<Person>, ISpecification<HirePersonFromUser>
{
    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Gender);
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return candidate != 0;
    }
}
