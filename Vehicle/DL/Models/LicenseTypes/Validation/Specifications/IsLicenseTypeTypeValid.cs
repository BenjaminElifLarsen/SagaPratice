using Common.SpecificationPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeTypeValid : ISpecification<EstablishLicenseTypeFromUser>
{
    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate.Type);
    }
}
