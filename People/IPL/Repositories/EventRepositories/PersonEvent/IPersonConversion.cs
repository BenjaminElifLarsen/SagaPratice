using Common.Events.Conversion;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
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
