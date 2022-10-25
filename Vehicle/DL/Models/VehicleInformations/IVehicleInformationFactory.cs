using Common.ResultPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.Validation;

namespace VehicleDomain.DL.Models.VehicleInformations;
internal interface IVehicleInformationFactory
{
    public Result<VehicleInformation> CreateVehicleInformation(AddVehicleInformation vehicleInformation, VehicleInformationValidationData validationData);
}
