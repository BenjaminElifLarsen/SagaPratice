using Common.ResultPattern;
using VehicleDomain.AL.Services.VehicleInformations.Queries;
using VehicleDomain.AL.Services.VehicleInformations.Queries.GetList;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService
{
    public async Task<Result<IEnumerable<VehicleInformationListItem>>> GetVehicleInformationListAsync()
    {
        var list = await Task.Run(() => _vehicleInformationRepository.AllAsync(new VehicleInformationListItemQuery()));
        return new SuccessResult<IEnumerable<VehicleInformationListItem>>(list);
    }
}
