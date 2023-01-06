using BaseRepository;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.AL.Services.People.Queries;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.CQRS.Queries;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.DL.Models;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.IPL.Context;
internal static class Seeder
{
    public static void MockSeedData(IPeopleContext peopleContext, IUnitOfWork unitOfWork, IPersonCommandBus commandBus)
    {
        //var c1 = new RecogniseGender("She", "Her");
        //commandBus.Dispatch(c1);
        //var c2 = new RecogniseGender("She", "They");
        //commandBus.Dispatch(c2);

        ////1) run c1 and c2 such that they are added to the context via the command bus. 2) get gender id. 3) run c3 with the id
        //var gender = unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result.First(); //problem: command bus and event bus have not registrated any routes as this is done per requet rather than at startup
        //var c3 = new HirePersonFromUser("Triss", "Nib", gender.Id, new(1956,1,2)); //the new pm repository will need to handle registrating to event, so that should not be to big of a problem (the repo is called by the command handlers)
        ////thus just need to figure out how to subscribe the commandhandler to the commandbus in this part of the code. For the commands just need to get the people registry as a parameter and call the specific setup method
        //commandBus.Dispatch(c3);

        Gender g1 = new("She", "Her"); //could in the end convert this to run through the system as intended via the services
        Gender g2 = new("She", "They");
        var g1Guid = Guid.NewGuid();
        var g2Guid = Guid.NewGuid();
        g1.AddDomainEvent(new GenderRecognisedSucceeded(g1, g1Guid, g1Guid));
        g2.AddDomainEvent(new GenderRecognisedSucceeded(g2, g2Guid, g2Guid));
        
        if (!unitOfWork.GenderRepository.AllAsync(new GenderIdQuery()).Result.Any())
        {
            peopleContext.Add(g1);
            peopleContext.Add(g2);
        }

        Person p1 = new(Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), "Triss", "Nib", new(1956, 1, 2), g1.Id);
        if (!unitOfWork.PersonRepository.AllAsync(new PersonListItemQuery()).Result.Any())
        { //remember the create event and events for adding a gender to a person and other way around
            peopleContext.Add(p1); //or set up command and dispatch them
            g1.AddPerson(p1.Id);
        }

        //peopleContext.Save(); //this does not run any events, will need the unit of work for that
        unitOfWork.Save();
    }
}
