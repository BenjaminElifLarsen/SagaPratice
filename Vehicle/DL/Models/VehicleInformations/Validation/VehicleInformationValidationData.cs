using VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation;
internal class VehicleInformationValidationData
{
    public IEnumerable<LicenseTypeValidation> LicenseTypes { get; private set; }
	public VehicleInformationValidationData(IEnumerable<LicenseTypeValidation> licenseTypes)
	{
		LicenseTypes = licenseTypes;
	}
}
