using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Errrors;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;

namespace PersonDomain.DL.Factories;
internal sealed class PersonFactory : IPersonFactory
{
    public Result<Person> CreatePerson(HirePersonFromUser person, PersonValidationData validationData)
    {
        List<string> errors = new();

        var flag = new PersonHireValidator(person, validationData).Validate();
        if(!flag)
        {
            errors.AddRange(PersonErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Person>(errors.ToArray());
        }

        Person entity = new(person.FirstName, person.LastName, new(person.Birth.Year, person.Birth.Month, person.Birth.Day), person.Gender);
        return new SuccessResult<Person>(entity);
    }
}
