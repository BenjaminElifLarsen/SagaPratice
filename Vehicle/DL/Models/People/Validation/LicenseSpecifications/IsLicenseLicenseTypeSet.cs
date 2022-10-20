using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;
internal class IsLicenseLicenseTypeSet : ISpecification<AddLicenseToPerson>
{
    public bool IsSatisfiedBy(AddLicenseToPerson candidate)
    {
        return candidate.LicenseType != 0;
    }
}
