using VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;
using LicenseTypeAgeValidation = VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels.LicenseTypeAgeValidation;

namespace VehicleDomain.DL.Models.People.Validation;
internal class PersonValidationData
{
    public IEnumerable<LicenseTypeAgeValidation> LicenseTypes { get; private set; }

	public PersonValidationData(IEnumerable<LicenseTypeAgeValidation> typeAgeValidations)	
	{
		LicenseTypes = typeAgeValidations;
	}
}


internal class PersonCreationLicenseValidationData
{
    public Dictionary<int, LicenseValidationData> LicenseTypes { get; private set; }
    public IEnumerable<LicenseTypeIdValidation> PermittedLicenseTypeIds { get; private set; }

    public PersonCreationLicenseValidationData(Dictionary<int, LicenseValidationData> licenseTypes, IEnumerable<LicenseTypeIdValidation> permittedIds)
    {
        LicenseTypes = licenseTypes;
        PermittedLicenseTypeIds = permittedIds;

    }

    internal class LicenseValidationData
    {
        public LicenseTypeAgeValidation AgeRequirement { get; private set; }

        public LicenseValidationData(LicenseTypeAgeValidation ageRequirement)
        {
            AgeRequirement = ageRequirement;
        }
    }
}