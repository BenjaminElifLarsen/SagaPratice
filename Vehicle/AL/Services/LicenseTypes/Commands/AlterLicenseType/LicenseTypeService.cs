using Common.ResultPattern;
using VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.AL.Services.LicenseTypes;
internal partial class LicenseTypeService
{
    public async Task<Result> AlterLicenseTypeAsync(AlterLicenseType command)
    {
        var processManager = _processManagers.SingleOrDefault(x => x is IAlterLicenseTypeProcessManager);
        if(processManager is not null)
        {
            processManager.SetUp(command.CommandId);
            processManager.RegistrateHandler(Handler);
        }
        await Task.Run(() => _commandBus.Dispatch(command));
        while (!CanReturnResult) ;
        return _result;
    }
}
