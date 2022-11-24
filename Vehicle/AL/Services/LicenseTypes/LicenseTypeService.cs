using VehicleDomain.AL.Busses.Command;
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Services.LicenseTypes;
internal partial class LicenseTypeService : ILicenseTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleCommandBus _commandBus;

    public LicenseTypeService(IUnitOfWork unitOfWork, IVehicleCommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }
}
