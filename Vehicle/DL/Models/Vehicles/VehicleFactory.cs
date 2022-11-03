using Common.Other;
using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Validation;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;

namespace VehicleDomain.DL.Models.Vehicles;
internal class VehicleFactory : IVehicleFactory
{
    public Result<Vehicle> CreateVehicle(AddVehicleWithNoOperator vehicle, VehicleValidationData validationData)
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

        Vehicle entity = new(vehicle.Produced, new(vehicle.VehicleInformation), new(""));
        return new SuccessResult<Vehicle>(entity);
    }

    public Result<Vehicle> CreateVehicle(AddVehicleWithOperators vehicle, VehicleValidationWithOperatorsData validationData)
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

        Vehicle entity = new(vehicle.Produced, new(vehicle.VehicleInformation), new(""));
        foreach(var @operator in vehicle.Operators)
        {
            entity.AddOperator(new(@operator));
        }        
        return new SuccessResult<Vehicle>(entity);
    }
}
