using VehicleDomain.AL.Handlers.Command;
using VehicleDomain.DL.Models.Operators;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _operatorRepository; //maybe call a domain service that contains the repository as this service is an application service
    private readonly IVehicleCommandHandler _vehicleCommandHandler;
    public OperatorService(IOperatorRepository operatorRepository, IVehicleCommandHandler vehicleCommandHandler)
    {
        _operatorRepository = operatorRepository;
        _vehicleCommandHandler = vehicleCommandHandler;
    }
}
