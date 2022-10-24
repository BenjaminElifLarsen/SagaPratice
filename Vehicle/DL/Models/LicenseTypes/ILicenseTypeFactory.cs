using Common.ResultPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes;
internal interface ILicenseTypeFactory
{
    public Result<LicenseType> CreateLicenseType(EstablishLicenseTypeFromUser licenseType);
}
