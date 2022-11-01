using VehicleDomain.DL.CQRS.Commands.Handlers;
using VehicleDomain.DL.Models.Operators;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;
    private readonly IVehicleCommandHandler _vehicleCommandHandler;
    public OperatorService(IOperatorRepository operatorRepository, IVehicleCommandHandler vehicleCommandHandler)
    {
        _operatorRepository = operatorRepository;
        _vehicleCommandHandler = vehicleCommandHandler;
    }
}
