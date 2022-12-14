using Common.Events.Domain;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
public interface IPeopleEventHandler : 
    IDomainEventHandler<PersonHiredSucceeded>,
    IDomainEventHandler<PersonFiredSucceeded>,
    IDomainEventHandler<PersonChangedGender>,
    IDomainEventHandler<PersonAddedToGenderSucceeded>,
    IDomainEventHandler<PersonRemovedFromGenderSucceeded>,
    IDomainEventHandler<GenderRecognisedSucceeded>,
    IDomainEventHandler<GenderUnrecognisedSucceeded>
{
}
