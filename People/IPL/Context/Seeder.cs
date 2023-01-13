using BaseRepository;
using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Services.People.Queries;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.CQRS.Queries;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Services;

namespace PersonDomain.IPL.Context;
internal static class Seeder
{
    public static void MockSeedData(IPersonContext peopleContext, IUnitOfWork unitOfWork, IPersonCommandBus commandBus)
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
        Person p2 = new(Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAB"), "Nia", "Nib", new(1993, 1, 2), g2.Id);
        var p1Guid = Guid.NewGuid();
        var p2Guid = Guid.NewGuid();
        p1.AddDomainEvent(new PersonHiredSucceeded(p1, p1Guid, p1Guid));
        p2.AddDomainEvent(new PersonHiredSucceeded(p2, p2Guid, p2Guid));
        g1.AddDomainEvent(new PersonAddedToGenderSucceeded(g1, p1.Id, p1Guid, p1Guid));
        g1.AddDomainEvent(new PersonAddedToGenderSucceeded(g1, p2.Id, p2Guid, p2Guid));
        g1.AddDomainEvent(new PersonRemovedFromGenderSucceeded(g1, p2.Id, p2Guid, p2Guid));
        g2.AddDomainEvent(new PersonAddedToGenderSucceeded(g2, p2.Id, p2Guid, p2Guid));
        p2.AddDomainEvent(new PersonChangedGender(p2, p2Guid, p2Guid));
        if (!unitOfWork.PersonRepository.AllAsync(new PersonListItemQuery()).Result.Any())
        { //remember the create event and events for adding a gender to a person and other way around
            peopleContext.Add(p1); //or set up commands and dispatch them
            peopleContext.Add(p2);
            g1.AddPerson(p1.Id);
            g2.AddPerson(p2.Id);
        }

        //peopleContext.Save(); //this does not run any events, will need the unit of work for that
        unitOfWork.Save();
    }
}
