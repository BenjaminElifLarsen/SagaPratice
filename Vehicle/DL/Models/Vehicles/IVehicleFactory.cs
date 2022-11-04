using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Validation;

namespace VehicleDomain.DL.Models.Vehicles;
public interface IVehicleFactory
{
    public Result<Vehicle> CreateVehicle(BuyVehicleWithNoOperator vehicle, VehicleValidationData validationData);
    public Result<Vehicle> CreateVehicle(BuyVehicleWithOperators vehicle, VehicleValidationWithOperatorsData validationData);
}
