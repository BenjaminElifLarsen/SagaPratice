using Common.ResultPattern;
using VehicleDomain.AL.CQRS.Queries;
using VehicleDomain.AL.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService
{
    public async Task<Result<IEnumerable<VehicleInformationListItem>>> GetVehicleInformationList()
    {
        var list = await Task.Run(() => _vehicleInformationRepository.AllAsync(new VehicleInformationListItemQuery()));
        return new SuccessResult<IEnumerable<VehicleInformationListItem>>(list);
    }
}
