﻿using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationLicenseTypeSat : ISpecification<AddVehicleInformation>
{
    public bool IsSatisfiedBy(AddVehicleInformation candidate)
    {
        throw new NotImplementedException();
    }
}
