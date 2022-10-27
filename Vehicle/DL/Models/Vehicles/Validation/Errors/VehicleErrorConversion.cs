using Common.Other;
using Common.SpecificationPattern;

namespace VehicleDomain.DL.Models.Vehicles.Validation.Errors;
internal class VehicleErrorConversion : IErrorConversion
{
    public static IEnumerable<string> Convert(BinaryFlag binaryFlag)
    {
        List<string> errors = new();
        if(binaryFlag == VehicleErrors.InvalidAmountOfOperator)
        {
            errors.Add($"Invalid amount of operators for vehicle.");
        }
        if (binaryFlag == VehicleErrors.InvalidOperator)
        {
            errors.Add($"Vehicle operator was not found.");
        }
        if (binaryFlag == VehicleErrors.InvalidVehicleInformation)
        {
            errors.Add($"Vehicle vehicle information was not found.");
        }
        if (binaryFlag == VehicleErrors.InvalidDistance)
        {
            errors.Add($"Invalid vehicle distance.");
        }
        if (binaryFlag == VehicleErrors.InvalidProductionDate)
        {
            errors.Add($"Invalid vehicle production date.");
        }
        return errors;
    }
}
