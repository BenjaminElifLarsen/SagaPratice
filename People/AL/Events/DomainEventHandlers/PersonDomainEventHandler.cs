using PeopleDomain.DL.Events.Domain;
using PeopleDomain.DL.Model;

namespace PeopleDomain.AL.Events.DomainEventHandlers;
internal class PersonDomainEventHandler : IPersonDomainEventHandler
{
    private readonly IPersonEventPublisher _personEventPublisher;

    public PersonDomainEventHandler(IPersonEventPublisher personEventPublisher)
    {
        _personEventPublisher = personEventPublisher;
    }
    //https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation
    public void PersonHiredDomainEvent(Person person) //text under 7-15 could be helpful, event handler trigger integration event
    {
        throw new NotImplementedException();
    }
}
