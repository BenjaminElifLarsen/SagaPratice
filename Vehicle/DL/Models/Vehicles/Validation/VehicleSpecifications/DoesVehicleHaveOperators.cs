using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class DoesVehicleHaveOperators : ISpecification<BuyVehicleWithOperators>, ISpecification<IEnumerable<IdReference>>
{
    public bool IsSatisfiedBy(BuyVehicleWithOperators candidate)
    {
        return IsSatisfiedBy(candidate.Operators);
    }

    public bool IsSatisfiedBy(IEnumerable<IdReference> candidate)
    {
        return IsSatisfiedBy(candidate.Select(x => x.Id));
    }

    private bool IsSatisfiedBy(IEnumerable<int> candidate)
    {
        return candidate.Any();
    }
}
