using Common.Events.Bus;
using Common.Events.Domain;
using PeopleDomain.DL.Events.Domain;

namespace PeopleDomain.AL.Handlers.Event;
public interface IPeopleEventHandler :
    IEventHandler<PersonHiredSucceeded>,
    IEventHandler<PersonFiredSucceeded>,
    IEventHandler<PersonChangedGender>,
    IEventHandler<PersonAddedToGenderSucceeded>,
    IEventHandler<PersonRemovedFromGenderSucceeded>,
    IEventHandler<GenderRecognisedSucceeded>,
    IEventHandler<GenderUnrecognisedSucceeded>
{
}
