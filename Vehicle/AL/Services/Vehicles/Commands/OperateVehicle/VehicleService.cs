using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService
{
    public async Task<Result> StartOperatingVehicleAsync(AttemptToStartVehicle command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        throw new NotImplementedException();//return;
    }

    public async Task<Result> StopOperatingVehicleAsync(StopOperatingVehicle command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        throw new NotImplementedException();//return;
    }
}
