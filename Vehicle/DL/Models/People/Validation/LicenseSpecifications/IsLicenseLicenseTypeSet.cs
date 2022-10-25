using Common.SpecificationPattern;
using VehicleDomain.DL.Models.People.CQRS.Commands;
using l = VehicleDomain.DL.Models.People.CQRS.Commands.License; // Without this one, License would point to the model in the People folder.

namespace VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;
internal class IsLicenseLicenseTypeSet : ISpecification<AddLicenseToPerson>, ISpecification<l>
{
    public bool IsSatisfiedBy(AddLicenseToPerson candidate)
    {
        return candidate.LicenseType != 0;
    }

    public bool IsSatisfiedBy(l candidate)
    {
        return candidate.LicenseTypeId != 0;
    }
}
