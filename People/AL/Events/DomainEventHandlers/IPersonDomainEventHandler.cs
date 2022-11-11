using PeopleDomain.DL.Model;

namespace PeopleDomain.AL.Events.DomainEventHandlers;
public interface IPersonDomainEventHandler
{
    public void PersonHiredDomainEvent(Person person);
}
