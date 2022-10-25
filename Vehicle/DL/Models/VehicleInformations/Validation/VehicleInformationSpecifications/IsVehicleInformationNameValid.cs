﻿using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationNameValid : ISpecification<AddVehicleInformationFromExternalSystem> 
{ 
    public bool IsSatisfiedBy(AddVehicleInformationFromExternalSystem candidate)
    { 
        throw new NotImplementedException();
    }
}
