using Common.BinaryFlags;
using Common.ResultPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.Validation;
using VehicleDomain.DL.Models.VehicleInformations.Validation.Errors;

namespace VehicleDomain.DL.Models.VehicleInformations;
internal class VehicleInformationFactory : IVehicleInformationFactory
{
    public Result<VehicleInformation> CreateVehicleInformation(AddVehicleInformationFromSystem vehicleInformation, VehicleInformationValidationData validationData)
    {
        List<string> errors = new();

        BinaryFlag flag = new VehicleInformationValidator(vehicleInformation, validationData).Validate();
        if(flag != 0)
        {
            errors.AddRange(VehicleInformationErrrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<VehicleInformation>(errors.ToArray());
        }

        VehicleInformation entity = new(vehicleInformation.VehicleName, vehicleInformation.MaxNumberOfWheel, new(vehicleInformation.LicenseTypeId));
        return new SuccessResult<VehicleInformation>(entity);
    }
}
