using Common.ProcessManager;
using Common.ResultPattern;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Services.LicenseTypes;
internal partial class LicenseTypeService : ILicenseTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleCommandBus _commandBus;
    private readonly IEnumerable<IProcessManager> _processManagers; //maybe have an extra interface to split up process mangers between the different modules.
    private Result? _result;

    public LicenseTypeService(IUnitOfWork unitOfWork, IVehicleCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
        _processManagers = processManagers;
    }

    private void Handler(ProcesserFinished @event) => _result = @event.Result;
    private bool CanReturnResult => _result is not null;
}
