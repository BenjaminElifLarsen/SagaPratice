using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Models;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
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
