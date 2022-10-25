using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal interface ILicenseTypeFactory
{
    public Result<LicenseType> CreateLicenseType(EstablishLicenseTypeFromUser licenseType);
}
