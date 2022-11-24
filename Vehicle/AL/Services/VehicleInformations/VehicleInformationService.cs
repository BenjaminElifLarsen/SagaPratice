using VehicleDomain.AL.Busses.Command;
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService : IVehicleInformationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleCommandBus _commandBus;
    public VehicleInformationService(IUnitOfWork unitOfWork, IVehicleCommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }
}
