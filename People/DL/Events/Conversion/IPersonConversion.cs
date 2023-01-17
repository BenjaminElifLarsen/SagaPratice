using Common.Events.Conversion;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.DL.Events.Conversion;
internal interface IPersonConversion :
    ISetEvent,
    IGetEvent<PersonHiredSucceeded>,
    IGetEvent<PersonFiredSucceeded>,
    IGetEvent<PersonChangedGender>,
    IGetEvent<PersonChangedBirth>,
    IGetEvent<PersonChangedLastName>,
    IGetEvent<PersonChangedFirstName>
{
}
