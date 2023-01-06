using API.Controllers;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PeopleDomain.AL.Registries;
using PeopleDomain.AL.Services.Genders;

namespace API.Middleware;

public class ServiceEventRegistryMiddleware
{
    private readonly RequestDelegate _next;

	public ServiceEventRegistryMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context, IEnumerable<IRoutingRegistry> registries, IGenderService genderService)
    {
        var methodsToRunOn = new string[] { "POST", "PUT", "PATCH" };
        var vehicleDomain = new string[] { nameof(OperatorController), nameof(VehicleController), nameof(VehicleInformationController), nameof(LicenseTypeController) };
        var peopleDomain = new string[] { nameof(GenderController), nameof(PersonController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;
        
        if(methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            switch (controllerName)
            {
                case nameof(GenderController):
                    var selected = registries.SingleOrDefault(x => x is IPersonRegistry) as IPersonRegistry;
                    selected.SetUpRouting(genderService);
                    break;
            }
        }

        await _next(context);
	}
}
