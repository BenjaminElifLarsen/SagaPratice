using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.Validation;
internal class VehicleValidationWithOperatorsData
{
    public IEnumerable<OperatorIdValidation> Operators { get; private set; }
	public IEnumerable<VehicleInformationIdValidation> VehicleInformations { get; private set; }
	public VehicleValidationWithOperatorsData(IEnumerable<OperatorIdValidation> operators, IEnumerable<VehicleInformationIdValidation> vehicleInformations)
	{
		Operators = operators;
		VehicleInformations = vehicleInformations;
	}
}

internal class VehicleValidationData
{
    public IEnumerable<VehicleInformationIdValidation> VehicleInformations { get; private set; }
    public VehicleValidationData(IEnumerable<VehicleInformationIdValidation> vehicleInformations)
    {
        VehicleInformations = vehicleInformations;
    }
}
