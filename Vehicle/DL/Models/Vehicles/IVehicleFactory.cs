using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Validation;

namespace VehicleDomain.DL.Models.Vehicles;
internal interface IVehicleFactory
{
    public Result<Vehicle> CreateVehicle(AddVehicleWithNoOperator vehicle, VehicleValidationData validationData);
    public Result<Vehicle> CreateVehicle(AddVehicleWithOperators vehicle, VehicleValidationData validationData);
}
