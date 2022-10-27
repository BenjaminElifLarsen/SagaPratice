﻿using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class IsVehicleVehicleInformationPermitted : ISpecification<AddVehicleWithOperators>, ISpecification<AddVehicleWithNoOperator>
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
        return _vehicleInformations.Any(x => x.Id == candidate);
    }
}
