using Common.Other;
using Common.SpecificationPattern;
using Common.SpecificationPattern.Composite.Extensions;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;
using VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;

namespace VehicleDomain.DL.Models.Vehicles.Validation;
internal class VehicleWithOperatorsCreationValidator : IValidate
{
    private BuyVehicleWithOperators _vehicle;
    private VehicleValidationWithOperatorsData _validationData;
    public VehicleWithOperatorsCreationValidator(BuyVehicleWithOperators vehicle, VehicleValidationWithOperatorsData validationData)
    {
        _vehicle = vehicle;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsVehicleVehicleInformationSat().And<BuyVehicleWithOperators>(new IsVehicleVehicleInformationPermitted(_validationData)).IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidVehicleInformation;
        flag += new DoesVehicleOperatorExist(_validationData).IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidOperator;
        flag += new DoesVehicleHaveOperators().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidAmountOfOperator;
        flag += new IsVehicleProductionDateValid().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidProductionDate;
        flag += new IsVehicleSerialNumberValid().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidSerialNumber;
        return flag;
    }
}

internal class VehicleWithNoOperatorCreationValidator : IValidate
{
    private BuyVehicleWithNoOperator _vehicle;
    private VehicleValidationData _validationData;

    public VehicleWithNoOperatorCreationValidator(BuyVehicleWithNoOperator vehicle, VehicleValidationData validationData)
    {
        _vehicle = vehicle;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsVehicleVehicleInformationSat().And<BuyVehicleWithNoOperator>(new IsVehicleVehicleInformationPermitted(_validationData)).IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidVehicleInformation;
        flag += new IsVehicleProductionDateValid().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidProductionDate;
        flag += new IsVehicleSerialNumberValid().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidSerialNumber;
        return flag;
    }
}
