using Common.ResultPattern;
using VehicleDomain.AL.Services.LicenseTypes.Queries;
using VehicleDomain.AL.Services.LicenseTypes.Queries.GetDetails;

namespace VehicleDomain.AL.Services.LicenseTypes;
internal partial class LicenseTypeService
{
    public async Task<Result<LicenseTypeDetails>> GetLicenseTypeDetailsAsync(Guid id)
    {
        var details = await _unitOfWork.LicenseTypeRepository.GetAsync(id, new LicenseTypeDetailsQuery());
        return new SuccessResult<LicenseTypeDetails>(details);
    }
}
