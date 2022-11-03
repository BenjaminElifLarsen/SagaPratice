using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationLicenseTypeSat : ISpecification<AddVehicleInformationFromSystem>
{
    public bool IsSatisfiedBy(AddVehicleInformationFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.LicenseTypeId);
    }

    private bool IsSatisfiedBy(int candidate)
    {
        return candidate > 0;
    }
}
