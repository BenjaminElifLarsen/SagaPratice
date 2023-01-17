using Common.Events.Conversion;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.DL.Events.Conversion;
internal interface IGenderConversion :
    ISetEvent,
    IGetEvent<GenderRecognisedSucceeded>,
    IGetEvent<GenderUnrecognisedSucceeded>,
    IGetEvent<PersonAddedToGenderSucceeded>,
    IGetEvent<PersonRemovedFromGenderSucceeded>
{
}
