using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Errrors;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;

namespace PersonDomain.DL.Factories;
internal sealed class GenderFactory : IGenderFactory
{
    public Result<Gender> CreateGender(RecogniseGender gender, GenderValidationData validationData)
    { //consider better name
        List<string> errors = new();

        var flag = new GenderValidator(gender, validationData).Validate();
        if (!flag)
        {
            errors.AddRange(GenderErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Gender>(errors.ToArray());
        }

        Gender entity = new(gender.VerbSubject, gender.VerbObject);
        return new SuccessResult<Gender>(entity);
    }
}
