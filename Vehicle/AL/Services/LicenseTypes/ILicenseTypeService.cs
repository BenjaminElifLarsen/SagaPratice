using Common.ResultPattern;
using VehicleDomain.AL.Services.LicenseTypes.Queries.GetDetails;
using VehicleDomain.AL.Services.LicenseTypes.Queries.GetList;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace VehicleDomain.AL.Services.LicenseTypes;

public interface ILicenseTypeService
{
    Task<Result<IEnumerable<LicenseTypeListItem>>> GetLicenseTypeListAsync();
    Task<Result<LicenseTypeDetails>> GetLicenseTypeDetailsAsync(Guid id);
    Task<Result> EstablishLicenseTypeAsync(EstablishLicenseTypeFromUser command);
    Task<Result> AlterLicenseTypeAsync(AlterLicenseType command);
    Task<Result> ObsoleteLicenseTypeAsync(ObsoleteLicenseTypeFromUser command);
}