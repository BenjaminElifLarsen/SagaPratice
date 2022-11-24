using VehicleDomain.AL.Busses.Command;
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleCommandBus _commandBus;
    public VehicleService(IUnitOfWork unitOfWork, IVehicleCommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }

}
