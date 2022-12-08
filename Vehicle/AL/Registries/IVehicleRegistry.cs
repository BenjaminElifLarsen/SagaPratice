using Common.Routing;
using VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;

namespace VehicleDomain.AL.Registries;
internal interface IVehicleRegistry : IRoutingRegistry
{
    public void SetUpRouting(IAlterLicenseTypeProcessManager processManager);
}
