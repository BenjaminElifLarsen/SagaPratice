using VehicleDomain.AL.Busses.Command;
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService : IOperatorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleCommandBus _commandBus;
    public OperatorService(IUnitOfWork unitOfWork, IVehicleCommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }
}
