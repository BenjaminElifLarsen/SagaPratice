using Common.ResultPattern;
using VehicleDomain.AL.Services.VehicleInformations.Queries.GetDetails;
using VehicleDomain.AL.Services.VehicleInformations.Queries.GetList;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.AL.Services.VehicleInformations;
public interface IVehicleInformationService
{//put the read models and queries into the CQRS folder under AL
    Task<Result<IEnumerable<VehicleInformationListItem>>> GetVehicleInformationListAsync();
    Task<Result<VehicleInformationDetails>> GetVehicleInformationDetailsAsync(Guid id);
    Task<Result> SetupVehicleInformation(AddVehicleInformationFromSystem command);
}
