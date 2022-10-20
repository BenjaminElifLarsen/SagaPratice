using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.Validation.PersonSpecifications;
internal class IsPersonIdSet : ISpecification<AddPersonNoLicenseFromSystem>, ISpecification<AddPersonWithLicenseFromUser>
{
    public bool IsSatisfiedBy(AddPersonWithLicenseFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Id);
    }

    public bool IsSatisfiedBy(AddPersonNoLicenseFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.Id);
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return candidate > 0;
    }
}
