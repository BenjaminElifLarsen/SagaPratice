using PeopleDomain.AL.Busses.Command;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
internal sealed class PeopleEventHandler : IPeopleEventHandler
{
    private readonly IPersonCommandBus _commandBus;
    public PeopleEventHandler(IPersonCommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Handle(PersonHiredSucceeded @event)
    {
        _commandBus.Dispatch(new AddPersonToGender(@event.AggregateId, @event.GenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonFiredSucceeded @event)
    {
        _commandBus.Dispatch(new RemovePersonFromGender(@event.AggregateId, @event.GenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonChangedGender @event)
    {
        _commandBus.Dispatch(new ChangePersonGender(@event.AggregateId, @event.NewGenderId, @event.OldGenderId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(PersonAddedToGenderSucceeded @event)
    {
        //log all events and commands. Let the busses do the logging
    }

    public void Handle(PersonRemovedFromGenderSucceeded @event)
    {
        //log all events and commands
    }

    public void Handle(GenderRecognisedSucceeded @event)
    {
        //log all events and commands
    }

    public void Handle(GenderUnrecognisedSucceeded @event)
    {
        //log all events and commands
    }
}
