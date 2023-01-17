using API.Controllers;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
using PersonDomain.AL.Registries;

namespace API.Middleware;

public class ProcessManagerRouterMiddleware
{
    private readonly RequestDelegate _next;

	public ProcessManagerRouterMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context, IEnumerable<IProcessManagerRouter> pmRoutes, IEnumerable<IRoutingRegistry> registries)
	{
        var methodsToRunOn = new string[] { "POST", "PUT", "PATCH" };
        var peopleDomain = new string[] { nameof(GenderController), nameof(PersonController) };
        var vehicleDomain = new string[] { nameof(OperatorController), nameof(VehicleController), nameof(VehicleInformationController), nameof(LicenseTypeController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;

        if(methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            if(peopleDomain.Any(x => string.Equals(x, controllerName)))
            {
                var selectedRegistry = registries.SingleOrDefault(x => x is IPersonRegistry) as IPersonRegistry;
                var genderRecognise = pmRoutes.SingleOrDefault(x => x is IGenderRecogniseProcessRouter) as IGenderRecogniseProcessRouter;
                selectedRegistry.SetUpRouting(genderRecognise);
                var genderUnrecognise = pmRoutes.SingleOrDefault(x => x is IGenderUnrecogniseProcessRouter) as IGenderUnrecogniseProcessRouter;
                selectedRegistry.SetUpRouting(genderUnrecognise);
            }
        }


        await _next(context);
	}
}
