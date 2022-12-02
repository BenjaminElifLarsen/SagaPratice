using Common.SpecificationPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Validation.GenderSpecifications;
internal sealed class CanGenderBeRemoved : ISpecification<Gender>
{ //gender cannot be removed as long time there are references to it
    public bool IsSatisfiedBy(Gender candidate)
    {
        throw new NotImplementedException();
    }
}
