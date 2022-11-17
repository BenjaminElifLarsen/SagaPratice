using VehicleDomain.AL.Handlers.Command;
using VehicleDomain.DL.Models.VehicleInformations;

namespace VehicleDomain.AL.Services.VehicleInformations;
public partial class VehicleInformationService : IVehicleInformationService
{
    private readonly IVehicleCommandHandler _vehicleCommandHandler;
    private readonly IVehicleInformationRepository _vehicleInformationRepository;

    public VehicleInformationService(IVehicleInformationRepository vehicleInformationRepository, IVehicleCommandHandler vehicleCommandHandler)
    {
        _vehicleInformationRepository = vehicleInformationRepository;
        _vehicleCommandHandler = vehicleCommandHandler;
    }
}
