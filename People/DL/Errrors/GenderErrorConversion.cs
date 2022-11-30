using Common.BinaryFlags;
using Common.SpecificationPattern;

namespace PeopleDomain.DL.Errrors;
internal class GenderErrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if(binaryFlag == GenderErrors.CannotBeRemoved)
        {
            errors.Add($"Gender cannot be removed.");
        }
        if (binaryFlag == GenderErrors.InvalidVerbObject)
        {
            errors.Add($"Invalid gender verb object.");
        }
        if (binaryFlag == GenderErrors.InvalidVerbSubject)
        {
            errors.Add($"Invalid gender verb subject.");
        }
        if (binaryFlag == GenderErrors.VerbObjectAndSubjectInUse)
        {
            errors.Add($"Invalid gender verb object and verb subject combination.");
        }
        return errors;
    }
}
