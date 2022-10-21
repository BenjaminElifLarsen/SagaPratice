using VehicleDomain.DL.Errors;

namespace VehicleDomain.DL.Models.People.Validation;
internal static class PersonErrorConversion
{
    public static IEnumerable<string> Convert(int binaryFlag)
    {
        List<string> errors = new();
        if((binaryFlag & (int)PersonErrors.IdNotSet) == (int)PersonErrors.IdNotSet)
        {
            errors.Add($"Person id not sat.");
        }
        if ((binaryFlag & (int)PersonErrors.InvalidAgeForLicense) == (int)PersonErrors.InvalidAgeForLicense)
        {
            errors.Add($"Invalid person age for license.");
        }
        if ((binaryFlag & (int)PersonErrors.InvalidBirth) == (int)PersonErrors.InvalidBirth)
        {
            errors.Add($"Invalid person birth.");
        }
        if ((binaryFlag & (int)PersonErrors.NotWithinAgeRange) == (int)PersonErrors.NotWithinAgeRange)
        {
            errors.Add($"Person not within license age requirements.");
        }
        return errors;
    }
}
