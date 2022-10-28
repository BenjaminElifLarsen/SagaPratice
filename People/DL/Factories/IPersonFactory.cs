using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Model;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
internal interface IPersonFactory
{
    public Result<Person> HirePerson(HirePersonFromUser person, PersonValidationData validationData);
}
