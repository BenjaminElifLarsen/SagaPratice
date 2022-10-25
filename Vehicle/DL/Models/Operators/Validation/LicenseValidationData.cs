using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Operators.Validation;
internal class LicenseValidationData
{
    public LicenseTypeAgeValidation LicenseYearRequirement { get; private set; }

    public LicenseValidationData(LicenseTypeAgeValidation licenseYearRequirement)
    {
        LicenseYearRequirement = licenseYearRequirement;
    }
}
