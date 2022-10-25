using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;
internal class IsOperatorIdSet : ISpecification<AddPersonNoLicenseFromSystem>, ISpecification<AddPersonWithLicenseFromUser>
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
