using Common.SpecificationPattern;
using VehicleDomain.DL.Models.People.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.Validation.PersonSpecifications;
internal class IsPersonOfValidAge : ISpecification<AddPersonNoLicenseFromSystem>, ISpecification<AddPersonWithLicenseFromUser>
{
    public bool IsSatisfiedBy(AddPersonWithLicenseFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(AddPersonNoLicenseFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(DateTime birth)
    {
        return birth <= DateTime.Now;
    }
}
