using Common.ResultPattern;
using VehicleDomain.AL.Services.Vehicles.Queries.GetList;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService
{
    public async Task<Result<IEnumerable<VehicleListItem>>> GetVehicleInUseListAsync()
    {
        var list = await _vehicleRepository.CurrentlyOperating(new VehicleListItemQuery());
        return new SuccessResult<IEnumerable<VehicleListItem>>(list);
    }

    public async Task<Result<IEnumerable<VehicleListItem>>> GetVehicleListAsync()
    {
        var list = await _vehicleRepository.AllAsync(new VehicleListItemQuery());
        return new SuccessResult<IEnumerable<VehicleListItem>>(list);
    }
}
