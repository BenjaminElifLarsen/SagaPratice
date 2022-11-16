using Common.Events.Domain;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
public interface IPeopleEventHandler : 
    IDomainEventHandler<PersonHired>,
    IDomainEventHandler<PersonFired>,
    IDomainEventHandler<PersonChangedGender>
{
}
