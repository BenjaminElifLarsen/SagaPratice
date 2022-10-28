using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Validation.PersonSpecifications;
internal class IsPersonGenderSat : ISpecification<Person>, ISpecification<HirePersonFromUser>
{
    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        throw new NotImplementedException();
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }
}
