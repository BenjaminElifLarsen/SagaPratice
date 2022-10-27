namespace VehicleDomain.DL.Models.Vehicles.Validation;
internal class VehicleValidationData
{
    public IEnumerable<IdReference> Operators { get; private set; }
	public IEnumerable<IdReference> VehicleInformations { get; private set; }
	public VehicleValidationData(IEnumerable<IdReference> operators, IEnumerable<IdReference> vehicleInformations)
	{
		Operators = operators;
		VehicleInformations = vehicleInformations;
	}
}
