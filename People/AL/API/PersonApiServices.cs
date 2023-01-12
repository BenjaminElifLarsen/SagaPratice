using BaseRepository;
using Common.Events.Store.Event;
using Common.Events.Store.ProcessManager;
using Common.ProcessManager;
using Common.RepositoryPattern;
using Common.RepositoryPattern.Events;
using Common.RepositoryPattern.ProcessManagers;
using Common.Routing;
using Microsoft.Extensions.DependencyInjection;
using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.Handlers.Command;
using PersonDomain.AL.Handlers.Event;
using PersonDomain.AL.ProcessManagers.Gender.Recognise;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
using PersonDomain.AL.ProcessManagers.Person.Fire;
using PersonDomain.AL.ProcessManagers.Person.Hire;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PersonDomain.AL.ProcessManagers.Routers.GenderRecogniseProcessRouter;
using PersonDomain.AL.Registries;
using PersonDomain.AL.Services.Genders;
using PersonDomain.AL.Services.People;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Context;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.EventRepositories;
using PersonDomain.IPL.Repositories.ProcesserManagers;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.API;
public class PersonApiServices
{
    public static void Add(IServiceCollection services)
    {
        services.AddSingleton<IPersonContext, MockPeopleContext>(); // Singleton because the data is stored in-memory.
        services.AddScoped<IBaseRepository<Gender>, MockBaseRepository<Gender, IPersonContext>>();
        services.AddScoped<IBaseRepository<Person>, MockBaseRepository<Person, IPersonContext>>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IGenderFactory, GenderFactory>();
        services.AddScoped<IPersonFactory, PersonFactory>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IGenderService, GenderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPersonCommandHandler, PersonCommandHandler>();
        services.AddScoped<IPeopleEventHandler, PeopleEventHandler>();
        services.AddScoped<IPersonCommandBus, MockCommandBus>();
        services.AddScoped<IPersonDomainEventBus, MockDomainEventBus>();
        services.AddScoped<IRoutingRegistry, PersonRegistry>();
        services.AddScoped<IProcessManager, PersonalInformationChangeProcessManager>();
        services.AddScoped<IProcessManager, FireProcessManager>();
        services.AddScoped<IProcessManager, HireProcessManager>();
        services.AddScoped<IProcessManager, UnrecogniseProcessManager>();
        services.AddScoped<IBaseProcessManagerRepository<GenderRecogniseProcessManager>, MockProcessManagerRepository<GenderRecogniseProcessManager, IPersonContext>>();
        services.AddScoped<IGenderRecogniseProcessRepository, GenderRecogniseProcessRepository>();
        services.AddScoped<IProcessManagerRouter, GenderRecogniseProcessRouter>();
        services.AddSingleton<IEventStore<Guid>, MockEventStore>();
        services.AddScoped<IBaseEventRepository<Guid>, MockEventRepository<Guid,IEventStore<Guid>>>(); //if similarly added to Operator domain, the dependency injector can inject the wrong mock repository, consider solulations
        services.AddScoped<IGenderEventRepository, GenderEventRepository>();
    }

    public static void Seed(IServiceProvider provider)
    {
        Seeder.MockSeedData(provider.CreateScope().ServiceProvider.GetService<IPersonContext>(), provider.CreateScope().ServiceProvider.GetService<IUnitOfWork>(), provider.CreateScope().ServiceProvider.GetService<IPersonCommandBus>());
    }
}
