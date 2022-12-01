using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService
{
    public async Task<Result> StartOperatingVehicleAsync(StartOperatingVehicle command)
    {
        return await Task.Run(() => _commandBus.Dispatch(command));
    }

    public async Task<Result> StopOperatingVehicleAsync(StopOperatingVehicle command)
    {
        return await Task.Run(() => _commandBus.Dispatch(command));
    }
}
