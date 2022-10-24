using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;
internal class IsLicenseTypeRenewPeriodValid : ISpecification<EstablishLicenseTypeFromUser>
{
    private readonly byte _renewPeriod;
    public IsLicenseTypeRenewPeriodValid(byte minimumRenewPeriod)
    {
        _renewPeriod = minimumRenewPeriod;
    }

    public bool IsSatisfiedBy(EstablishLicenseTypeFromUser candidate)
    {
        return candidate.RenewPeriod >= _renewPeriod;
    }
}
