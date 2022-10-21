using VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.People.Validation;
internal class LicenseValidationData
{
    public LicenseTypeAgeValidation LicenseYearRequirement { get; private set; }

	public LicenseValidationData(LicenseTypeAgeValidation licenseYearRequirement)
	{
		LicenseYearRequirement = licenseYearRequirement;
	}
}
