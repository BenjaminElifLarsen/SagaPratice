using Common.SpecificationPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeRenewPeriodValid : ISpecification<EstablishLicenseTypeFromUser>, ISpecification<AlterLicenseType>
{
    private readonly byte _renewPeriod;
    public IsLicenseTypeRenewPeriodValid(byte minimumRenewPeriod)
    {
        _renewPeriod = minimumRenewPeriod;
    }

    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return IsSatisfiedBy(candidate.RenewPeriod);
    }

    public bool IsSatisfiedBy(AlterLicenseType candidate)
    {
        return candidate.RenewPeriod is null || IsSatisfiedBy(candidate.RenewPeriod.RenewPeriod);
    }

    private bool IsSatisfiedBy(byte candidate)
    {
        return candidate >= _renewPeriod;
    }
}
