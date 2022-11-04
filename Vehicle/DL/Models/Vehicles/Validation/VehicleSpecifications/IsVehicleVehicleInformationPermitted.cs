using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleVehicleInformationPermitted : ISpecification<BuyVehicleWithOperators>, ISpecification<BuyVehicleWithNoOperator>
{
    private readonly IEnumerable<VehicleInformationIdValidation> _vehicleInformations;

    public IsVehicleVehicleInformationPermitted(VehicleValidationWithOperatorsData validationData)
    {
        _vehicleInformations = validationData.VehicleInformations;
    }

    public IsVehicleVehicleInformationPermitted(VehicleValidationData validationData)
    {
        _vehicleInformations = validationData.VehicleInformations;
    }

    public bool IsSatisfiedBy(BuyVehicleWithNoOperator candidate)
    {
        return IsSatisfiedBy(candidate.VehicleInformation);
    }

    public bool IsSatisfiedBy(BuyVehicleWithOperators candidate)
    {
        return IsSatisfiedBy(candidate.VehicleInformation);
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return _vehicleInformations.Any(x => x.Id == candidate);
    }
}
