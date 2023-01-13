using Common.Events.Bus;
using Common.Events.Domain;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.AL.Handlers.Event;
public interface IPeopleEventHandler :
    IEventHandler<PersonHiredSucceeded>,
    IEventHandler<PersonFiredSucceeded>,
    IEventHandler<PersonReplacedGender>,
    IEventHandler<PersonAddedToGenderSucceeded>,
    IEventHandler<PersonRemovedFromGenderSucceeded>,
    IEventHandler<GenderRecognisedSucceeded>,
    IEventHandler<GenderUnrecognisedSucceeded>
{
}
