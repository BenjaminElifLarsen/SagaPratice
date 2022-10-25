using Common.Other;
using Common.SpecificationPattern;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.Errors;
internal class VehicleInformationErrrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if(binaryFlag == VehicleInformationErrors.InvalidLicenseType)
        {
            errors.Add($"Invalid vehicle information license type.");
        }
        if (binaryFlag == VehicleInformationErrors.InvalidName)
        {
            errors.Add($"Invalid vehicle information name.");
        }
        if (binaryFlag == VehicleInformationErrors.InvalidWheelAmount)
        {
            errors.Add($"Invalid vehicle information number of wheels.");
        }
        return errors;
    }
}
