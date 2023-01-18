using BaseRepository;
using Common.Events.Store.Event;
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
using PersonDomain.AL.ProcessManagers.Routers.GenderUnrecogniseProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonFireProcessRouter;
using PersonDomain.AL.ProcessManagers.Routers.PersonHireProcessRouter;
using PersonDomain.AL.Registries;
using PersonDomain.AL.Services.Genders;
using PersonDomain.AL.Services.People;
using PersonDomain.DL.Factories;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Context;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
using PersonDomain.IPL.Repositories.ProcesserManagers.Genders;
using PersonDomain.IPL.Repositories.ProcesserManagers.People;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.API;
public class PersonApiServices
{
    public static void Add(IServiceCollection services)
    {
        Context(services);
        Factories(services);
        UnitOfWork(services);
        DomainRepos(services);
        Handlers(services);
        Events(services);
        ProcessManagers(services);
    }

    private static void Context(IServiceCollection services)
    {
        services.AddSingleton<IPersonContext, MockPeopleContext>(); // Singleton because the data is stored in-memory.
    }

    private static void Factories(IServiceCollection services)
    {
        services.AddScoped<IGenderFactory, GenderFactory>();
        services.AddScoped<IPersonFactory, PersonFactory>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IGenderService, GenderService>();
    }

    private static void UnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void DomainRepos(IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<Gender>, MockBaseRepository<Gender, IPersonContext>>();
        services.AddScoped<IBaseRepository<Person>, MockBaseRepository<Person, IPersonContext>>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
    }

    private static void Handlers(IServiceCollection services)
    {
        services.AddScoped<IPersonCommandHandler, PersonCommandHandler>();
        services.AddScoped<IPeopleEventHandler, PeopleEventHandler>();
        services.AddScoped<IPersonCommandBus, MockCommandBus>();
        services.AddScoped<IPersonDomainEventBus, MockDomainEventBus>();
        services.AddScoped<IRoutingRegistry, PersonRegistry>();
    }

    private static void Events(IServiceCollection services)
    {
        services.AddSingleton<IEventStore<Guid>, MockEventStore>();
        services.AddScoped<IBaseEventRepository<Guid>, MockEventRepository<Guid, IEventStore<Guid>>>(); //if similarly added to Vehicle domain, the dependency injector can inject the wrong mock repository, consider solulations
        services.AddScoped<IGenderEventRepository, GenderEventRepository>();
        services.AddScoped<IPersonEventRepository, PersonEventRepository>();
    }

    private static void ProcessManagers(IServiceCollection services)
    {
        services.AddScoped<IProcessManager, PersonalInformationChangeProcessManager>(); //old design


        services.AddScoped<IBaseProcessManagerRepository<GenderRecogniseProcessManager>, MockProcessManagerRepository<GenderRecogniseProcessManager, IPersonContext>>();
        services.AddScoped<IGenderRecogniseProcessRepository, GenderRecogniseProcessRepository>();

        services.AddScoped<IBaseProcessManagerRepository<GenderUnrecogniseProcessManager>, MockProcessManagerRepository<GenderUnrecogniseProcessManager, IPersonContext>>();
        services.AddScoped<IGenderUnrecogniseProcessRepository, GenderUnrecogniseProcessRepository>();

        services.AddScoped<IBaseProcessManagerRepository<FireProcessManager>, MockProcessManagerRepository<FireProcessManager, IPersonContext>>();
        services.AddScoped<IPersonFireProcessRepository, PersonFireProcessRepository>();

        services.AddScoped<IBaseProcessManagerRepository<HireProcessManager>, MockProcessManagerRepository<HireProcessManager, IPersonContext>>();
        services.AddScoped<IPersonHireProcessRepository, PersonHireProcessRepository>();

        services.AddScoped<IProcessManagerRouter, GenderRecogniseProcessRouter>();
        services.AddScoped<IProcessManagerRouter, GenderUnrecogniseProcessRouter>();
        services.AddScoped<IProcessManagerRouter, PersonFireProcessRouter>();
        services.AddScoped<IProcessManagerRouter, PersonHireProcessRouter>();
    }


    public static void Seed(IServiceProvider provider)
    {
        var serviceProvider = provider.CreateScope().ServiceProvider;
        Seeder.MockSeedData(serviceProvider.GetService<IPersonContext>(), 
            serviceProvider.GetService<IUnitOfWork>(), 
            serviceProvider.GetService<IPersonCommandBus>(),
            serviceProvider.GetService<IEnumerable<IRoutingRegistry>>(),
            serviceProvider.GetService<IEnumerable<IProcessManagerRouter>>());
    }
}
