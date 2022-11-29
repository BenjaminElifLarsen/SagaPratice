using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
internal class PeopleEventHandler : IPeopleEventHandler
{
    private readonly IPeopleCommandBus _commandBus;
    public PeopleEventHandler(IPeopleCommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Handle(PersonHired @event)
    {
        _commandBus.Publish(new AddPersonToGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonFired @event)
    {
        _commandBus.Publish(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonChangedGender @event)
    {
        _commandBus.Publish(new ChangePersonGender(@event.Data.PersonId, @event.Data.NewGenderId, @event.Data.OldGenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonAddedToGenderSuccessed @event)
    {
        //log all events and commands. Let the busses do the logging
    }

    public void Handle(PersonRemovedFromGenderSuccessed @event)
    {
        //log all events and commands
    }

    public void Handle(GenderRecognised @event)
    {
        //log all events and commands
    }

    public void Handle(GenderUnrecognised @event)
    {
        //log all events and commands
    }
}
