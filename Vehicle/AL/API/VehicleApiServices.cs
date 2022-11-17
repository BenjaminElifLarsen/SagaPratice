using BaseRepository;
using Common.RepositoryPattern;
using Microsoft.Extensions.DependencyInjection;
using VehicleDomain.AL.Services.Operators;
using VehicleDomain.AL.Services.Vehicles;
using VehicleDomain.AL.Services.VehicleInformations;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;
using VehicleDomain.IPL.Context;
using VehicleDomain.IPL.Services;
using VehicleDomain.AL.Handlers.Command;

namespace VehicleDomain.AL.API;

public class VehicleApiServices
{
    public static void Add(IServiceCollection services)
    {
        services.AddSingleton<IVehicleContext, MockVehicleContext>();
        services.AddScoped<IBaseRepository<Operator>, MockBaseRepository<Operator, IVehicleContext, IVehicleContext>>();
        services.AddScoped<IBaseRepository<Vehicle>, MockBaseRepository<Vehicle, IVehicleContext, IVehicleContext>>();
        services.AddScoped<IBaseRepository<LicenseType>, MockBaseRepository<LicenseType, IVehicleContext, IVehicleContext>>();
        services.AddScoped<IBaseRepository<VehicleInformation>, MockBaseRepository<VehicleInformation, IVehicleContext, IVehicleContext>>();
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void Seed(IServiceProvider provider)
    {
        Seeder.MockSeedData(provider.CreateScope().ServiceProvider.GetService<IVehicleContext>());
    }
}
