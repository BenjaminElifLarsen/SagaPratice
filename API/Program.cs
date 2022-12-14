using API.Controllers;
using API.Middleware;
using Common.Other;
using Common.Other.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using PersonDomain.AL;
using PersonDomain.AL.API;
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
PersonApiServices.Add(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    VehicleApiServices.Seed(app.Services);
    PersonApiServices.Seed(app.Services);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RegistryMiddleware>();
app.UseMiddleware<ProcessManagerMiddleware>();
app.UseMiddleware<ProcessManagerRouterMiddleware>();
app.UseMiddleware<ServiceEventRegistryMiddleware>();

app.MapControllers();

app.Run();
