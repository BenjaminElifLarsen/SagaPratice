using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;
internal class IsOperatorOfValidAge : ISpecification<AddOperatorNoLicenseFromSystem>, ISpecification<AddOperatorWithLicenseFromUser>
{
    public bool IsSatisfiedBy(AddOperatorWithLicenseFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(AddOperatorNoLicenseFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(DateTime birth)
    {
        return birth <= DateTime.Now;
    }
}
