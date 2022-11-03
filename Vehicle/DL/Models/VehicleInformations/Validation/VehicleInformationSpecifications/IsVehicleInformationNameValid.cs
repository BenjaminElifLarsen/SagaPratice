﻿using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationNameValid : ISpecification<AddVehicleInformationFromSystem> 
{ 
    public bool IsSatisfiedBy(AddVehicleInformationFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.VehicleName);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
