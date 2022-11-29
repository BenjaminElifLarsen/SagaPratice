using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;
internal class IsOperatorIdSet : ISpecification<AddOperatorNoLicenseFromSystem>/*, ISpecification<AddOperatorWithLicenseFromUser>*/
{
    //public bool IsSatisfiedBy(AddOperatorWithLicenseFromUser candidate)
    //{
    //    return IsSatisfiedBy(candidate.Id);
    //}

    public bool IsSatisfiedBy(AddOperatorNoLicenseFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.Id);
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return candidate > 0;
    }
}
