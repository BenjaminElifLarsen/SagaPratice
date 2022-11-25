using Common.SpecificationPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeAgeRequirementValid : ISpecification<EstablishLicenseTypeFromUser>, ISpecification<AlterLicenseType>
{
    private readonly byte _ageRequirement;

    public IsLicenseTypeAgeRequirementValid(byte minimumAgeRequirement)
    {
        _ageRequirement = minimumAgeRequirement;
    }

    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return IsSatisfiedBy(candidate.AgeRequirement);
    }

    public bool IsSatisfiedBy(AlterLicenseType candidate)
    {
        return candidate.AgeRequirement is null || IsSatisfiedBy(candidate.AgeRequirement.AgeRequirement);
    }

    private bool IsSatisfiedBy(byte candidate)
    {
        return candidate >= _ageRequirement;
    }

}
