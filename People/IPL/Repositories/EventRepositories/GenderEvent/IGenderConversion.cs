using Common.Events.Conversion;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
internal interface IGenderConversion : 
    ISetEvent,
    IGetEvent<GenderRecognisedSucceeded>,
    IGetEvent<GenderUnrecognisedSucceeded>,
    IGetEvent<PersonAddedToGenderSucceeded>,
    IGetEvent<PersonRemovedFromGenderSucceeded>
{
}
