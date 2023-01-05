using Common.ResultPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService
{
    public async Task<Result> SetupVehicleInformation(AddVehicleInformationFromSystem command)
    {
        await Task.Run(() =>_commandBus.Dispatch(command));
        throw new NotImplementedException();//return;
    }
}
