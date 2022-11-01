using VehicleDomain.DL.Models.Operators;

namespace VehicleDomain.AL.Services;
public partial class VehicleService : IVehicleService
{
	private readonly IOperatorRepository _operatorRepository;
	public VehicleService(IOperatorRepository operatorRepository)
	{
		_operatorRepository = operatorRepository;
	}
}
