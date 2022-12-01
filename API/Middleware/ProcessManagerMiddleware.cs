using API.Controllers;
using Common.CQRS.Commands;
using Common.ProcessManager;
using Common.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using PeopleDomain.AL;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using VehicleDomain.AL;

namespace API.Middleware;

public class ProcessManagerMiddleware
{
    private readonly RequestDelegate _next;


    public ProcessManagerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IEnumerable<IProcessManager> processManagers)
    {
        var methodsToRunOn = new string[] { "POST", "PUT", "PATCH" };
        var peopleDomain = new string[] { nameof(GenderController), nameof(PersonController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;

        if (methodsToRunOn.Any(x => string.Equals(x, method)))
        { //mayhaps instead of all of this inject into the controllers all ProcessManagers and then each method find the correct and send it with the service method, where it get set up and all that
            ICommand command; //or, better, inject them into the service.
            //it just leave the question of how to set up the registrations, inject whatever class that will do that in services too?
            //getting data seems like it will require reading the context.Request.Boy, which is a stream
            if (peopleDomain.Any(x => string.Equals(x, controllerName)))
            { 
                IProcessManager selected;
                if (controllerActionDescriptor.ActionName == nameof(PersonController.ChangePersonalInformation))
                    selected = processManagers.SingleOrDefault(x => x is IPersonalInformationChangeProcessManager);
                //selected.SetUp();
            }
        }

        await _next(context);
    }
}
