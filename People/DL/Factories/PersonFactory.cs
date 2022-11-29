using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Models;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
internal class PersonFactory : IPersonFactory
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

        Person entity = new(person.FirstName, person.LastName, new(person.Birth.Year, person.Birth.Month, person.Birth.Day), new(person.Gender));
        return new SuccessResult<Person>(entity);
    }
}
