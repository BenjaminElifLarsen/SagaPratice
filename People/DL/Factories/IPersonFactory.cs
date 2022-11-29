using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Models;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
internal interface IPersonFactory
{
    public Result<Person> CreatePerson(HirePersonFromUser person, PersonValidationData validationData);
} //originally called HirePerson, changed to CreatePerson since the method creates a Person entity. Not sure if it is best like that or not, since it is the context that saves the person and they are hired by then
