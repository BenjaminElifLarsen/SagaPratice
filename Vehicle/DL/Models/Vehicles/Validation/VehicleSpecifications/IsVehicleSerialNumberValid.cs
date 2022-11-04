using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleSerialNumberValid : ISpecification<BuyVehicleWithNoOperator>, ISpecification<BuyVehicleWithOperators>
{
    public bool IsSatisfiedBy(BuyVehicleWithOperators candidate)
    {
        return IsSatisfiedBy(candidate.SerialNumber);
    }

    public bool IsSatisfiedBy(BuyVehicleWithNoOperator candidate)
    {
        return IsSatisfiedBy(candidate.SerialNumber);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
