using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.Validation;
public class VehicleValidationWithOperatorsData
{
    internal IEnumerable<OperatorIdValidation> Operators { get; private set; }
    internal IEnumerable<VehicleInformationIdValidation> VehicleInformations { get; private set; }
    internal VehicleValidationWithOperatorsData(IEnumerable<OperatorIdValidation> operators, IEnumerable<VehicleInformationIdValidation> vehicleInformations)
	{
		Operators = operators;
		VehicleInformations = vehicleInformations;
	}
}

public class VehicleValidationData
{
    internal IEnumerable<VehicleInformationIdValidation> VehicleInformations { get; private set; }
    internal VehicleValidationData(IEnumerable<VehicleInformationIdValidation> vehicleInformations)
    {
        VehicleInformations = vehicleInformations;
    }
}
