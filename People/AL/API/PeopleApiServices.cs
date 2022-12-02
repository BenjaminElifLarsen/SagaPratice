using BaseRepository;
using Common.ProcessManager;
using Common.RepositoryPattern;
using Common.Routing;
using Microsoft.Extensions.DependencyInjection;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.Busses.Event;
using PeopleDomain.AL.Handlers.Command;
using PeopleDomain.AL.Handlers.Event;
using PeopleDomain.AL.ProcessManagers.Person.Fire;
using PeopleDomain.AL.ProcessManagers.Person.Hire;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PeopleDomain.AL.Registries;
using PeopleDomain.AL.Services.Genders;
using PeopleDomain.AL.Services.People;
using PeopleDomain.DL.Factories;
using PeopleDomain.DL.Models;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.API;
public class PeopleApiServices
{
    public static void Add(IServiceCollection services)
    {
        services.AddSingleton<IPeopleContext, MockPeopleContext>(); // Singleton because the data is stored in-memory.
        services.AddScoped<IBaseRepository<Gender>, MockBaseRepository<Gender, IPeopleContext, IPeopleContext>>();
        services.AddScoped<IBaseRepository<Person>, MockBaseRepository<Person, IPeopleContext, IPeopleContext>>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IGenderFactory, GenderFactory>();
        services.AddScoped<IPersonFactory, PersonFactory>();
        services.AddScoped<IPeopleService, PeopleService>();
        services.AddScoped<IGenderService, GenderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPeopleCommandHandler, PeopleCommandHandler>();
        services.AddScoped<IPeopleEventHandler, PeopleEventHandler>();
        services.AddScoped<IPeopleCommandBus, MockCommandBus>();
        services.AddScoped<IPeopleDomainEventBus, MockDomainEventBus>();
        services.AddScoped<IRoutingRegistry, PeopleRegistry>();
        services.AddScoped<IProcessManager, PersonalInformationChangeProcessManager>();
        services.AddScoped<IProcessManager, FireProcessManager>();
        services.AddScoped<IProcessManager, HireProcessManager>();
    }

    public static void Seed(IServiceProvider provider)
    {
        Seeder.MockSeedData(provider.CreateScope().ServiceProvider.GetService<IPeopleContext>());
    }
}
