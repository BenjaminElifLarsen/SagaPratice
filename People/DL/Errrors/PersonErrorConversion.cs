using Common.Other;
using Common.SpecificationPattern;

namespace PeopleDomain.DL.Errrors;
internal class PersonErrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if(binaryFlag == PersonErrors.InvalidFirstName)
        {
            errors.Add($"Invalid person first name.");
        }
        if (binaryFlag == PersonErrors.InvalidLastName)
        {
            errors.Add($"Invalid person last name.");
        }
        if (binaryFlag == PersonErrors.InvalidGender)
        {
            errors.Add($"Invalid person gender.");
        }
        if (binaryFlag == PersonErrors.InvalidBirth)
        {
            errors.Add($"Invalid person birth.");
        }
        return errors;
    }
}
