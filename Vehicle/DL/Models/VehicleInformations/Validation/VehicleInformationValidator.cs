using Common.Other;
using Common.SpecificationPattern;
using Common.SpecificationPattern.Composite.Extensions;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.Validation.Errors;
using VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation;
internal class VehicleInformationValidator : IValidate
{
    private readonly AddVehicleInformationFromSystem _vehicleInformation;
    private readonly VehicleInformationValidationData _validationData;
    public VehicleInformationValidator(AddVehicleInformationFromSystem vehicleInformation, VehicleInformationValidationData validationData)
    {
        _vehicleInformation = vehicleInformation;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsVehicleInformationLicenseTypeSat().And(new IsVehicleInformationLicenseTypeValid(_validationData)).IsSatisfiedBy(_vehicleInformation) ? 0 : VehicleInformationErrors.InvalidLicenseType;
        flag += new IsVehicleInformationNameValid().IsSatisfiedBy(_vehicleInformation) ? 0 : VehicleInformationErrors.InvalidName;
        flag += new IsVehicleInformationWheelAmountSat().IsSatisfiedBy(_vehicleInformation) ? 0 : VehicleInformationErrors.InvalidWheelAmount;
        return flag;
    }
}
