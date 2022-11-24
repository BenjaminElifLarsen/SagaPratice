using Common.ResultPattern;
using VehicleDomain.AL.Services.VehicleInformations.Queries;
using VehicleDomain.AL.Services.VehicleInformations.Queries.GetDetails;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService
{
    public async Task<Result<VehicleInformationDetails>> GetVehicleInformationDetailsAsync(int id)
    {
        var details = await _unitOfWork.VehicleInformationRepository.GetAsync(id, new VehicleInformationDetailsQuery());
        if(details is null)
        {
            return new NotFoundResult<VehicleInformationDetails>("Not found.");
        }
        return new SuccessResult<VehicleInformationDetails>(details);
    }
}
