using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.AL.Services.Vehicles;
public partial class VehicleService : IVehicleService
{
    private readonly IVehicleFactory _vehicleFactory;
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleFactory vehicleFactory, IVehicleRepository vehicleRepository)
    {
        _vehicleFactory = vehicleFactory;
        _vehicleRepository = vehicleRepository;
    }

}
