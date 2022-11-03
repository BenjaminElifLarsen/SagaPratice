using BaseRepository;
using Common.RepositoryPattern;
using Microsoft.Extensions.DependencyInjection;
using VehicleDomain.AL.Services.Operators;
using VehicleDomain.AL.Services.Vehicles;
using VehicleDomain.AL.Services.VehicleInformations;
using VehicleDomain.DL.CQRS.Commands.Handlers;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;
using VehicleDomain.IPL.Context;

namespace VehicleDomain.AL.API;

public class VehicleApiServices
{
    public static void Add(IServiceCollection services)
    {
        services.AddSingleton<MockVehicleContext>();
        services.AddScoped<IBaseRepository<Operator>, MockBaseRepository<Operator, MockVehicleContext>>();
        services.AddScoped<IBaseRepository<Vehicle>, MockBaseRepository<Vehicle, MockVehicleContext>>();
        services.AddScoped<IBaseRepository<LicenseType>, MockBaseRepository<LicenseType, MockVehicleContext>>();
        services.AddScoped<IBaseRepository<VehicleInformation>, MockBaseRepository<VehicleInformation, MockVehicleContext>>();
        services.AddScoped<IOperatorRepository, OperatorRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<ILicenseTypeRepository, LicenseTypeRepository>();
        services.AddScoped<IVehicleInformationRepository, VehicleInformationRepository>();
        services.AddScoped<IOperatorFactory, OperatorFactory>();
        services.AddScoped<IVehicleFactory, VehicleFactory>();
        services.AddScoped<ILicenseTypeFactory, LicenseTypeFactory>();
        services.AddScoped<IVehicleInformationFactory, VehicleInformationFactory>();
        services.AddScoped<IVehicleCommandHandler, VehicleCommandHandler>();
        services.AddScoped<IOperatorService, OperatorService>();
        services.AddScoped<IVehicleInformationService, VehicleInformationService>();
        services.AddScoped<IVehicleService, VehicleService>();
    }

    public static void Seed(IServiceProvider provider)
    {
        Seeder.MockSeedData(provider.CreateScope().ServiceProvider.GetService<MockVehicleContext>());
    }
}
