using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;
using VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.People.Validation.LicenseSpecifications;
internal class IsLicenseArquiredValid : ISpecification<AddLicenseToPerson>
{
	private readonly LicenseTypeAgeValidation _licenseType;

	public IsLicenseArquiredValid(LicenseTypeAgeValidation licenseType)
	{
		_licenseType = licenseType;
	}

	public bool IsSatisfiedBy(AddLicenseToPerson candidate)
	{
		return IsSatisfiedBy(candidate.Arquired);
	}

	private bool IsSatisfiedBy(DateTime candidate)
	{
        var now = DateTime.Now;
        var age = (now.Year - candidate.Year - 1) +
        (((now.Month > candidate.Month) ||
        ((now.Month == candidate.Month) && (now.Day >= candidate.Day))) ? 1 : 0);
		return age < _licenseType.YearRequirement;
    }
}
