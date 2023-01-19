using API.Controllers;
using Common.CQRS.Commands;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using VehicleDomain.AL.Process_Managers.LicenseType.AlterLicenseType;
using VehicleDomain.AL.Process_Managers.Vehicle.StartVehicle;
using VehicleDomain.AL.Registries;

namespace API.Middleware;

public class ProcessManagerMiddleware
{
    private readonly RequestDelegate _next;


    public ProcessManagerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IEnumerable<IProcessManager> processManagers, IEnumerable<IRoutingRegistry> registries)
    {
        var methodsToRunOn = new string[] { "POST", "PUT", "PATCH" };
        var vehicleDomain = new string[] { nameof(OperatorController), nameof(VehicleController), nameof(VehicleInformationController), nameof(LicenseTypeController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;

        if (methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            if (vehicleDomain.Any(x => string.Equals(x, controllerName)))
            {
                var selectedRegistry = registries.SingleOrDefault(x => x is IVehicleRegistry) as IVehicleRegistry;
                var alterPM = processManagers.SingleOrDefault(x => x is IAlterLicenseTypeProcessManager) as IAlterLicenseTypeProcessManager;
                selectedRegistry.SetUpRouting(alterPM);
                var startPM = processManagers.SingleOrDefault(x => x is IStartVehicleProcessManager) as IStartVehicleProcessManager;
                selectedRegistry.SetUpRouting(startPM);
            }
        }

        await _next(context);
    }
}
