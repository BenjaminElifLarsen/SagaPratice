using API.Controllers;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PeopleDomain.AL.Registries;
using VehicleDomain.AL.Registries;

namespace API.Middleware;

public class RegistryMiddleware
{
    private readonly RequestDelegate _next;

    public RegistryMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IEnumerable<IRoutingRegistry> registries, IEnumerable<IProcessManager> processManagers)
    {
        var methodsToRunOn = new string[] { "POST", "PUT", "PATCH" };
        var vehicleDomain = new string[] { nameof(OperatorController), nameof(VehicleController), nameof(VehicleInformationController), nameof(LicenseTypeController) };
        var peopleDomain = new string[] { nameof(GenderController), nameof(PersonController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;
        if(methodsToRunOn.Any(x => string.Equals(x, method))) 
        { 
            if (vehicleDomain.Any(x => string.Equals(x, controllerName)))
            {
                var selected = registries.SingleOrDefault(x => x.GetType() == typeof(VehicleRegistry)); //could have an domain interface for each domain registry instead of relying on a concrete type
                selected.SetUpRouting();
            }
            else if (peopleDomain.Any(x => string.Equals(x, controllerName)))
            {
                var selected = registries.SingleOrDefault(x => x is IPersonRegistry) as IPersonRegistry;
                selected.SetUpRouting();
            }
        }
        await _next(context);
    }
}
