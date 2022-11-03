using VehicleDomain.DL.CQRS.Commands.Handlers;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService : IVehicleService
{
    private readonly IVehicleCommandHandler _vehicleCommandHandler;
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository, IVehicleCommandHandler vehicleCommandHandler)
    {
        _vehicleRepository = vehicleRepository;
        _vehicleCommandHandler = vehicleCommandHandler;
    }

}
