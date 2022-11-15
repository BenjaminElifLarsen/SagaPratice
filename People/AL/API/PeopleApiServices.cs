using BaseRepository;
using Common.Events.Domain;
using Common.Other;
using Common.RepositoryPattern;
using Microsoft.Extensions.DependencyInjection;
using PeopleDomain.AL.CQRS.Commands.Handlers;
using PeopleDomain.AL.Services.Genders;
using PeopleDomain.AL.Services.People;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.DL.Factories;
using PeopleDomain.DL.Model;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.API;
public class PeopleApiServices
{
    public static void Add(IServiceCollection services)
    {
        services.AddSingleton<MockPeopleContext>();
        services.AddScoped<IBaseRepository<Gender>, MockBaseRepository<Gender, MockPeopleContext>>();
        services.AddScoped<IBaseRepository<Person>, MockBaseRepository<Person, MockPeopleContext>>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IGenderFactory, GenderFactory>();
        services.AddScoped<IPersonFactory, PersonFactory>();
        services.AddScoped<IPeopleCommandHandler, PeopleCommandHandler>();
        services.AddScoped<IPeopleService, PeopleService>();
        services.AddScoped<IGenderService, GenderService>();
        services.AddScoped<IDomainEventBus, MockDomainEventBus>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void Seed(IServiceProvider provider)
    {
        Seeder.MockSeedData(provider.CreateScope().ServiceProvider.GetService<MockPeopleContext>());
    }
}
