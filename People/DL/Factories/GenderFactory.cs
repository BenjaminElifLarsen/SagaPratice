using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;
using PeopleDomain.DL.Errrors;
using PeopleDomain.DL.Model;
using PeopleDomain.DL.Validation;

namespace PeopleDomain.DL.Factories;
internal class GenderFactory : IGenderFactory
{
    public Result<Gender> CreateGender(PermitGender gender, GenderValidationData validationData)
    { //consider better name
        List<string> errors = new();

        var flag = new GenderValidator(gender, validationData).Validate();
        if (!flag)
        {
            errors.AddRange(GenderErrorConversion.Convert(flag));
        }
        throw new NotImplementedException();
    }
}
