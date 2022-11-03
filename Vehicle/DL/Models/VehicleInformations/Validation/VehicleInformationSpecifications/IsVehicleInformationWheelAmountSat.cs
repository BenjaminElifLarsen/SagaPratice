﻿using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationWheelAmountSat : ISpecification<AddVehicleInformationFromSystem>
{
    public bool IsSatisfiedBy(AddVehicleInformationFromSystem candidate)
    {
        throw new NotImplementedException();
    }
}
