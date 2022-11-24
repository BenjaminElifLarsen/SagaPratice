using Common.ResultPattern;
using VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;
using VehicleDomain.AL.Services.Vehicles.Queries.GetList;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Services.Vehicles;
public interface IVehicleService
{
    Task<Result<IEnumerable<VehicleListItem>>> GetVehicleListAsync();
    Task<Result<IEnumerable<VehicleListItem>>> GetVehicleInUseListAsync();
    Task<Result<VehicleDetails>> GetVehicleDetailsAsync(int id);
    Task<Result> StartOperatingVehicle(StartOperatingVehicle command);
    Task<Result> StopOperatingVehicle(StopOperatingVehicle command);
    Task<Result> BuyVehicleNoOperator(BuyVehicleWithNoOperator command);
    //Task<Result> BuyVehicleWithOperator(BuyVehicleWithOperators command);
}
