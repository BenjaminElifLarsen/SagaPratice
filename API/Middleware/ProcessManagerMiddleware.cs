using API.Controllers;
using Common.CQRS.Commands;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PersonDomain.AL.ProcessManagers.Gender.Recognise;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
using PersonDomain.AL.ProcessManagers.Person.Fire;
using PersonDomain.AL.ProcessManagers.Person.Hire;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PersonDomain.AL.Registries;
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
        var peopleDomain = new string[] { nameof(GenderController), nameof(PersonController) };
        var vehicleDomain = new string[] { nameof(OperatorController), nameof(VehicleController), nameof(VehicleInformationController), nameof(LicenseTypeController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;

        if (methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            if (peopleDomain.Any(x => string.Equals(x, controllerName)))
            {
                var selectedRegistry = registries.SingleOrDefault(x => x is IPersonRegistry) as IPersonRegistry;
                var changePM = processManagers.SingleOrDefault(x => x is IPersonalInformationChangeProcessManager) as IPersonalInformationChangeProcessManager;
                selectedRegistry.SetUpRouting(changePM); 
            }
            else if (vehicleDomain.Any(x => string.Equals(x, controllerName)))
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
