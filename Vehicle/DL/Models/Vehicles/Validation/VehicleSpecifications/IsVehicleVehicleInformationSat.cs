using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleVehicleInformationSat : ISpecification<BuyVehicleWithOperators>, ISpecification<BuyVehicleWithNoOperator>
{
    public bool IsSatisfiedBy(BuyVehicleWithNoOperator candidate)
    {
        return IsSatisfiedBy(candidate.VehicleInformation);
    }

    public bool IsSatisfiedBy(BuyVehicleWithOperators candidate)
    {
        return IsSatisfiedBy(candidate.VehicleInformation);
    }

    private bool IsSatisfiedBy(Guid candidate)
    {
        return candidate != Guid.Empty;
    }
}