using API.Controllers;
using Common.Other;
using Common.Other.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using PeopleDomain.AL;
using PeopleDomain.AL.API;
using VehicleDomain.AL;
using VehicleDomain.AL.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

VehicleApiServices.Add(builder.Services);
PeopleApiServices.Add(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    VehicleApiServices.Seed(app.Services);
    PeopleApiServices.Seed(app.Services);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<TestMiddleware>();

app.MapControllers();

app.Run();

public class TestMiddleware //move when working
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _provider;

    public TestMiddleware(RequestDelegate next, IServiceProvider provider)
    {
        _next = next;
        _provider = provider;
    }

    public async Task Invoke(HttpContext context, IEnumerable<IRoutingRegistry> registries)
    {
        var methodsToRunOn = new string[] { "POST", "PUT", "PATCH" };
        var vehicleDomain = new string[] { nameof(OperatorController) };
        var peopleDomain = new string[] { nameof(GenderController), nameof(PersonController) };

        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var method = context.Request.Method;

        if(vehicleDomain.Any(x => string.Equals(x,controllerName)) && methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            var selected = registries.SingleOrDefault(x => x.GetType() == typeof(VehicleRegistry)); //could have an domain interface for registry
            selected.SetUpRouting();
        }
        else if (peopleDomain.Any(x => string.Equals(x, controllerName)) && methodsToRunOn.Any(x => string.Equals(x, method)))
        {
            var selected = registries.SingleOrDefault(x => x.GetType() == typeof(PeopleRegistry));
            selected.SetUpRouting();
        }
        {

        }
        await _next(context);
    }
}