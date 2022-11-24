using Common.ResultPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService
{
    public async Task<Result> SetupVehicleInformation(AddVehicleInformationFromSystem command)
    {
        return await Task.Run(() =>_commandBus.Publish(command));
    }
}
