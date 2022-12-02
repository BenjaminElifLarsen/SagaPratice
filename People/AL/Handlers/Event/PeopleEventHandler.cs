using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
internal sealed class PeopleEventHandler : IPeopleEventHandler
{
    private readonly IPeopleCommandBus _commandBus;
    public PeopleEventHandler(IPeopleCommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Handle(PersonHiredSuccessed @event)
    {
        _commandBus.Dispatch(new AddPersonToGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonFiredSuccessed @event)
    {
        _commandBus.Dispatch(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.GenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonChangedGender @event)
    {
        _commandBus.Dispatch(new ChangePersonGender(@event.Data.PersonId, @event.Data.NewGenderId, @event.Data.OldGenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonAddedToGenderSuccessed @event)
    {
        //log all events and commands. Let the busses do the logging
    }

    public void Handle(PersonRemovedFromGenderSuccessed @event)
    {
        //log all events and commands
    }

    public void Handle(GenderRecognisedSuccessed @event)
    {
        //log all events and commands
    }

    public void Handle(GenderUnrecognisedSuccessed @event)
    {
        //log all events and commands
    }
}
