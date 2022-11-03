using Common.ResultPattern;
using VehicleDomain.AL.CQRS.Queries;
using VehicleDomain.AL.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService
{
    public async Task<Result<VehicleInformationDetails>> GetVehicleInformationDetailsAsync(int id)
    {
        var details = await _vehicleInformationRepository.GetAsync(id, new VehicleInformationDetailsQuery());
        if(details is null)
        {
            return new NotFoundResult<VehicleInformationDetails>("Not found.");
        }
        return new SuccessResult<VehicleInformationDetails>(details);
    }
}
