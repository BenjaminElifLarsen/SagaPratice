using Common.SpecificationPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeTypeValid : ISpecification<EstablishLicenseTypeFromUser>, ISpecification<AlterLicenseType>
{
    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Type);
    }

    public bool IsSatisfiedBy(AlterLicenseType candidate)
    {
        return candidate.Type is null || IsSatisfiedBy(candidate.Type.Type);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}
