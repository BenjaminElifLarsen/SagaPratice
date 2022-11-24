using Common.ResultPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService
{
    public async Task<Result> BuyVehicleNoOperator(BuyVehicleWithNoOperator command)
    {
        return await Task.Run(() => _commandBus.Publish(command));
    }

    public async Task<Result> BuyVehicleWithOperator(BuyVehicleWithOperators command)
    {
        return await Task.Run(() => _commandBus.Publish(command));
    }
}
