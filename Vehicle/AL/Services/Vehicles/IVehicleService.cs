using Common.ResultPattern;
using VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;
using VehicleDomain.AL.Services.Vehicles.Queries.GetList;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Services.Vehicles;
public interface IVehicleService
{
    Task<Result<IEnumerable<VehicleListItem>>> GetVehicleListAsync();
    Task<Result<IEnumerable<VehicleListItem>>> GetVehicleInUseListAsync();
    Task<Result<VehicleDetails>> GetVehicleDetailsAsync(Guid id);
    Task<Result> StartOperatingVehicleAsync(AttemptToStartVehicle command);
    Task<Result> StopOperatingVehicleAsync(StopOperatingVehicle command);
    Task<Result> BuyVehicleNoOperatorAsync(BuyVehicleWithNoOperator command);
    //Task<Result> BuyVehicleWithOperator(BuyVehicleWithOperators command);
}
