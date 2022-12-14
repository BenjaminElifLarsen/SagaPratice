using Common.Routing;
using VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
using VehicleDomain.AL.Process_Managers.Vehicle.StartVehicle;

namespace VehicleDomain.AL.Registries;
public interface IVehicleRegistry : IRoutingRegistry
{
    public void SetUpRouting(IAlterLicenseTypeProcessManager processManager);
    public void SetUpRouting(IStartVehicleProcessManager processManager);
}
