using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;

namespace VehicleDomain.DL.Models.People.Validation;
internal class PersonErrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if((binaryFlag.IsFlagPresent((int)PersonErrors.IdNotSet)))
        {
            errors.Add($"Person id not sat.");
        }
        if ((binaryFlag.IsFlagPresent((int)PersonErrors.InvalidAgeForLicense)))
        {
            errors.Add($"Invalid person age for license.");
        }
        if ((binaryFlag.IsFlagPresent((int)PersonErrors.InvalidBirth)))
        {
            errors.Add($"Invalid person birth.");
        }
        if ((binaryFlag.IsFlagPresent((int)PersonErrors.NotWithinAgeRange)))
        {
            errors.Add($"Person not within license age requirements.");
        }
        return errors;
    }
}