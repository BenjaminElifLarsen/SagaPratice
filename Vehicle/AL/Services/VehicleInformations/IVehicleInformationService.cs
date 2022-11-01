using Common.ResultPattern;
using VehicleDomain.AL.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services.VehicleInformations;
public interface IVehicleInformationService
{//put the read models and queries into the CQRS folder under AL
    Task<Result<IEnumerable<VehicleInformationListItem>>> GetVehicleInformationList();
}
