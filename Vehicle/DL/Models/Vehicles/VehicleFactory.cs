﻿using Common.BinaryFlags;
using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Validation;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;

namespace VehicleDomain.DL.Models.Vehicles;
internal class VehicleFactory : IVehicleFactory
{
    public Result<Vehicle> CreateVehicle(BuyVehicleWithNoOperator vehicle, VehicleValidationData validationData)
    {
        List<string> errors = new();

        BinaryFlag flag = new VehicleWithNoOperatorCreationValidator(vehicle, validationData).Validate();
        if(flag != 0)
        {
            errors.AddRange(VehicleErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Vehicle>(errors.ToArray());
        }

        Vehicle entity = new(vehicle.Produced, vehicle.VehicleInformation, new(vehicle.SerialNumber));
        return new SuccessResult<Vehicle>(entity);
    }

    public Result<Vehicle> CreateVehicle(BuyVehicleWithOperators vehicle, VehicleValidationWithOperatorsData validationData)
    {
        List<string> errors = new();

        BinaryFlag flag = new VehicleWithOperatorsCreationValidator(vehicle, validationData).Validate();
        if (flag != 0)
        {
            errors.AddRange(VehicleErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Vehicle>(errors.ToArray());
        }

        Vehicle entity = new(vehicle.Produced, vehicle.VehicleInformation, new(vehicle.SerialNumber));
        foreach(var @operator in vehicle.Operators)
        {
            entity.AddOperator(@operator);
        }        
        return new SuccessResult<Vehicle>(entity);
    }
}
