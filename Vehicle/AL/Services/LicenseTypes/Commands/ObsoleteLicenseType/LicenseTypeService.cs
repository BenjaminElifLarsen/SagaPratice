using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.AL.Services.LicenseTypes;
internal partial class LicenseTypeService
{
    public async Task<Result> ObsoleteLicenseTypeAsync(ObsoleteLicenseTypeFromUser command)
    {
        return await Task.Run(() => _commandBus.Dispatch(command));
    }
}
