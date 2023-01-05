using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationLicenseTypeValid : ISpecification<AddVehicleInformationFromSystem>
{
    private readonly IEnumerable<Guid> _licenseTypeIds;
    public IsVehicleInformationLicenseTypeValid(VehicleInformationValidationData validationData)
    {
        _licenseTypeIds = validationData.LicenseTypes.Select(x => x.Id);
    }

    public bool IsSatisfiedBy(AddVehicleInformationFromSystem candidate)
    {
        return IsSatisfiedBy(candidate.LicenseTypeId);
    }

    private bool IsSatisfiedBy(Guid candidate)
    {
        return _licenseTypeIds.Any(x => x == candidate);
    }
}
