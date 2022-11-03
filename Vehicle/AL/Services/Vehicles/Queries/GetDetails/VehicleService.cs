using Common.ResultPattern;
using VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService
{
    public async Task<Result<VehicleDetails>> GetVehicleDetailsAsync(int id)
    {
        var details = await _vehicleRepository.GetAsync(id, new VehicleDetailsQuery());
        if(details is null)
        {
            return new NotFoundResult<VehicleDetails>("Not found.");
        }
        return new SuccessResult<VehicleDetails>(details);
        //return details is not null ? new SuccessResult<VehicleDetails>(details) : new NotFoundResult<VehicleDetails>("Not found");
    }
}
