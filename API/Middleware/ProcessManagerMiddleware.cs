using API.Controllers;
using Common.CQRS.Commands;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;
using PeopleDomain.AL.ProcessManagers.Gender.Unrecognise;
using PeopleDomain.AL.ProcessManagers.Person.Fire;
using PeopleDomain.AL.ProcessManagers.Person.Hire;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PeopleDomain.AL.Registries;
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
                var selectedRegistry = registries.SingleOrDefault(x => x is IPeopleRegistry) as IPeopleRegistry;
                var changePM = processManagers.SingleOrDefault(x => x is IPersonalInformationChangeProcessManager) as IPersonalInformationChangeProcessManager;
                selectedRegistry.SetUpRouting(changePM); //consider moving all related to the process managers over to their own middleware, this class should only care about process managers
                var firePM = processManagers.SingleOrDefault(x => x is IFireProcessManager) as IFireProcessManager;
                selectedRegistry.SetUpRouting(firePM);
                var hirePM = processManagers.SingleOrDefault(x => x is IHireProcessManager) as IHireProcessManager;
                selectedRegistry.SetUpRouting(hirePM);
                var regPM = processManagers.SingleOrDefault(x => x is IRecogniseProcessManager) as IRecogniseProcessManager;
                selectedRegistry.SetUpRouting(regPM);
                var unregPM = processManagers.SingleOrDefault(x => x is IUnrecogniseProcessManager) as IUnrecogniseProcessManager;
                selectedRegistry.SetUpRouting(unregPM);
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
