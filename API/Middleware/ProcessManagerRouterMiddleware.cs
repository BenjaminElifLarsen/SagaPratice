using API.Controllers;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonChangeInformationProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
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
        var actionName = controllerActionDescriptor.ActionName;
        if(methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            if(peopleDomain.Any(x => string.Equals(x, controllerName)))
            { //improve upon this at some point
                var selectedRegistry = registries.Single(x => x is IPersonRegistry) as IPersonRegistry;
                var genderRecognise = pmRoutes.Single(x => x is IGenderRecogniseProcessRouter) as IGenderRecogniseProcessRouter;
                selectedRegistry.SetUpRouting(genderRecognise);
                var genderUnrecognise = pmRoutes.Single(x => x is IGenderUnrecogniseProcessRouter) as IGenderUnrecogniseProcessRouter;
                selectedRegistry.SetUpRouting(genderUnrecognise);
                var personFire = pmRoutes.Single(x => x is IPersonFireProcessRouter) as IPersonFireProcessRouter;
                selectedRegistry.SetUpRouting(personFire);
                var personChange = pmRoutes.Single(x => x is IPersonChangeInformationProcessRouter) as IPersonChangeInformationProcessRouter;
                selectedRegistry.SetUpRouting(personChange);
                if (nameof(PersonController.Hire) == actionName) //use switch case
                {
                    var personHire = pmRoutes.Single(x => x is IPersonHireProcessRouter) as IPersonHireProcessRouter;
                    selectedRegistry.SetUpRouting(personHire);
                }
            }
        }


        await _next(context);
	}
}
