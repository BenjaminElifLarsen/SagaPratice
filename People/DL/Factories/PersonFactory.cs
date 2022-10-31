using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Model;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
internal class PersonFactory : IPersonFactory
{
    public Result<Person> HirePerson(HirePersonFromUser person, PersonValidationData validationData)
    {
        List<string> errors = new();

        var flag = new PersonValidator(person, validationData).Validate();
        if(!flag)
        {
            errors.AddRange(PersonErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Person>(errors.ToArray());
        }

        Person entity = new(person.FirstName, person.LastName, person.Birth, new(person.Gender));
        return new SuccessResult<Person>(entity);
    }
}
