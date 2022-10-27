using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleDistanceMovedPositiveOrZero : ISpecification<AddDistanceToVehicleDistance>, ISpecification<ResetVehicleMovedDistance>
{
    public bool IsSatisfiedBy(ResetVehicleMovedDistance candidate)
    {
        return IsSatisfiedBy(candidate.NewDistance);
    }

    public bool IsSatisfiedBy(AddDistanceToVehicleDistance candidate)
    {
        return IsSatisfiedBy(candidate.DistanceToAdd);
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return candidate >= 0;
    }
}
