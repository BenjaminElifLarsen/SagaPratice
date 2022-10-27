using Common.Other;
using Common.SpecificationPattern;
using Common.SpecificationPattern.Composite.Extensions;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;
using VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;

namespace VehicleDomain.DL.Models.Vehicles.Validation;
internal class VehicleWithOperatorsCreationValidator : IValidate
{
    private AddVehicleWithOperators _vehicle;
    private VehicleValidationWithOperatorsData _validationData;
    public VehicleWithOperatorsCreationValidator(AddVehicleWithOperators vehicle, VehicleValidationWithOperatorsData validationData)
    {
        _vehicle = vehicle;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsVehicleVehicleInformationSat().And<AddVehicleWithOperators>(new IsVehicleVehicleInformationPermitted(_validationData)).IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidVehicleInformation;
        flag += new DoesVehicleOperatorExist(_validationData).IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidOperator;
        flag += new DoesVehicleHaveOperators().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidAmountOfOperator;
        flag += new IsVehicleProductionDateValid().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidProductionDate;
        return flag;
    }
}

internal class VehicleWithNoOperatorCreationValidator : IValidate
{
    private AddVehicleWithNoOperator _vehicle;
    private VehicleValidationData _validationData;

    public VehicleWithNoOperatorCreationValidator(AddVehicleWithNoOperator vehicle, VehicleValidationData validationData)
    {
        _vehicle = vehicle;
        _validationData = validationData;
    }

    public BinaryFlag Validate()
    {
        BinaryFlag flag = new();
        flag += new IsVehicleVehicleInformationSat().And<AddVehicleWithNoOperator>(new IsVehicleVehicleInformationPermitted(_validationData)).IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidVehicleInformation;
        flag += new IsVehicleProductionDateValid().IsSatisfiedBy(_vehicle) ? 0 : VehicleErrors.InvalidProductionDate;
        return flag;
    }
}
