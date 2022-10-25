using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
using LicenseTypeAgeValidation = VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels.LicenseTypeAgeValidation;

namespace VehicleDomain.DL.Models.Operators.Validation;
internal class OperatorValidationData
{
    public IEnumerable<LicenseTypeAgeValidation> LicenseTypes { get; private set; }

    public OperatorValidationData(IEnumerable<LicenseTypeAgeValidation> typeAgeValidations)
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