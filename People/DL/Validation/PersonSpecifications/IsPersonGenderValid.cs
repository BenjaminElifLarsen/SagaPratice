using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Validation.PersonSpecifications;
internal class IsPersonGenderValid : ISpecification<Person>, ISpecification<HirePersonFromUser>
{ //consider a better name

    public IsPersonGenderValid(PersonValidationData validationData)
    {

    }

    public bool IsSatisfiedBy(HirePersonFromUser candidate)
    {
        throw new NotImplementedException();
    }

    public bool IsSatisfiedBy(Person candidate)
    {
        throw new NotImplementedException();
    }
}
