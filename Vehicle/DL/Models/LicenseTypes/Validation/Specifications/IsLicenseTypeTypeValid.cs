using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeTypeValid : ISpecification<EstablishLicenseTypeFromUser>
{
    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate.Type);
    }
}
