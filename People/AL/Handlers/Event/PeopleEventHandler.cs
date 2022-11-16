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
    {
        _commandBus.Publish(new AddPersonToGender(@event.Data.PersonId, @event.Data.PersonId));
    }

    public void Handle(PersonFired @event)
    {
        throw new NotImplementedException();
    }

    public void Handle(PersonChangedGender @event)
    {
        throw new NotImplementedException();
    }
}
