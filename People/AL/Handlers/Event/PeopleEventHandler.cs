using Common.CQRS.Commands;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
internal class PeopleEventHandler : IPeopleEventHandler
{
    private readonly ICommandBus _commandBus;
    public PeopleEventHandler(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Handle(PersonHired @event)
    { //event class should not return any data
        _commandBus.Publish(new AddPersonToGender(@event.Data.PersonId, @event.Data.GenderId));
    }

    public void Handle(PersonFired @event)
    {
        _commandBus.Publish(new RemovePersonFromGender(@event.Data.PersonId, @event.Data.GenderId));
    }

    public void Handle(PersonChangedGender @event)
    {
        _commandBus.Publish(new ChangePersonGender(@event.Data.PersonId, @event.Data.NewGenderId, @event.Data.OldGenderId));
    }
}
