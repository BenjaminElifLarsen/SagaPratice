using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleVehicleInformationSat : ISpecification<AddVehicleWithOperators>, ISpecification<AddVehicleWithNoOperator>
{
    public bool IsSatisfiedBy(AddVehicleWithNoOperator candidate)
    {
        return IsSatisfiedBy(candidate.VehicleInformation);
    }

    public bool IsSatisfiedBy(AddVehicleWithOperators candidate)
    {
        return IsSatisfiedBy(candidate.VehicleInformation);
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return candidate != 0;
    }
}