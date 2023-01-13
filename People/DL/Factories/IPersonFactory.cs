using Common.Events.Domain;
using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;

namespace PersonDomain.DL.Factories;
internal interface IPersonFactory
{
    public Result<Person> CreatePerson(HirePersonFromUser person, PersonValidationData validationData);
    public Person HydratePerson(IEnumerable<DomainEvent> events);
} //originally called HirePerson, changed to CreatePerson since the method creates a Person entity. Not sure if it is best like that or not, since it is the context that saves the person and they are hired by then
