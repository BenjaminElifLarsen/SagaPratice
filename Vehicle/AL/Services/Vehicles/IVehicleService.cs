using Common.ResultPattern;
using VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;
using VehicleDomain.AL.Services.Vehicles.Queries.GetList;

namespace VehicleDomain.AL.Services.Vehicles;
public interface IVehicleService
{
    Task<Result<IEnumerable<VehicleListItem>>> GetVehicleListAsync();
    Task<Result<IEnumerable<VehicleListItem>>> GetVehicleInUseListAsync();
    Task<Result<VehicleDetails>> GetVehicleDetailsAsync(int id);
}
