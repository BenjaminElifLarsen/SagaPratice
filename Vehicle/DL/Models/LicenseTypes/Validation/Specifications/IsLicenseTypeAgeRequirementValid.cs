using Common.SpecificationPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeAgeRequirementValid : ISpecification<EstablishLicenseTypeFromUser>
{
    private readonly byte _ageRequirement;

    public IsLicenseTypeAgeRequirementValid(byte minimumAgeRequirement)
    {
        _ageRequirement = minimumAgeRequirement;
    }

    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return candidate.AgeRequirement >= _ageRequirement;
    }
}
