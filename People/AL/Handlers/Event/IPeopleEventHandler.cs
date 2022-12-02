using Common.Events.Domain;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
public interface IPeopleEventHandler : 
    IDomainEventHandler<PersonHiredSuccessed>,
    IDomainEventHandler<PersonFiredSuccessed>,
    IDomainEventHandler<PersonChangedGender>,
    IDomainEventHandler<PersonAddedToGenderSuccessed>,
    IDomainEventHandler<PersonRemovedFromGenderSuccessed>,
    IDomainEventHandler<GenderRecognisedSuccessed>,
    IDomainEventHandler<GenderUnrecognisedSuccessed>
{
}
