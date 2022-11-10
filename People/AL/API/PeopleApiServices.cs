using BaseRepository;
using Common.RepositoryPattern;
using Microsoft.Extensions.DependencyInjection;
using PeopleDomain.AL.Services.Genders;
using PeopleDomain.AL.Services.People;
using PeopleDomain.DL.CQRS.Commands.Handlers;
using PeopleDomain.DL.Factories;
using PeopleDomain.DL.Model;
using PeopleDomain.IPL.Context;
using PeopleDomain.IPL.Repositories;

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
    }

    public static void Seed(IServiceProvider provider)
    {
        Seeder.MockSeedData(provider.CreateScope().ServiceProvider.GetService<MockPeopleContext>());
    }
}
