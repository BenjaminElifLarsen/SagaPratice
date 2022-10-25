using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;

namespace VehicleDomain.DL.Models.Operators.Validation;
internal class OperatorErrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if (binaryFlag == (int)OperatorErrors.IdNotSet)
        {
            errors.Add($"Person id not sat.");
        }
        if (binaryFlag == (int)OperatorErrors.InvalidAgeForLicense)
        {
            errors.Add($"Invalid person age for license.");
        }
        if (binaryFlag == (int)OperatorErrors.InvalidBirth)
        {
            errors.Add($"Invalid person birth.");
        }
        if (binaryFlag == (int)OperatorErrors.NotWithinAgeRange)
        {
            errors.Add($"Person not within license age requirements.");
        }
        return errors;
    }
}