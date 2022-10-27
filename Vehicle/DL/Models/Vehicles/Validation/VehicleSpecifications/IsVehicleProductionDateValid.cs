using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleProductionDateValid : ISpecification<AddVehicleWithOperators>, ISpecification<AddVehicleWithNoOperator>
{ //should also check that it is not default datetime
    public bool IsSatisfiedBy(AddVehicleWithNoOperator candidate)
    {
        return IsSatisfiedBy(candidate.Produced);
    }

    public bool IsSatisfiedBy(AddVehicleWithOperators candidate)
    {
        return IsSatisfiedBy(candidate.Produced);
    }

    private bool IsSatisfiedBy(DateTime candidate)
    {
        return candidate <= DateTime.Now && candidate != default;
    }
}
