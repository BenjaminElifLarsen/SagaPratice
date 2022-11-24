using Common.ResultPattern;
using VehicleDomain.AL.Services.LicenseTypes.Queries;
using VehicleDomain.AL.Services.LicenseTypes.Queries.GetList;

namespace VehicleDomain.AL.Services.LicenseTypes;
internal partial class LicenseTypeService
{
    public async Task<Result<IEnumerable<LicenseTypeListItem>>> GetLicenseTypeListAsync()
    {
        var list = await _unitOfWork.LicenseTypeRepository.AllAsync(new LicenseTypeListItemQuery());
        return new SuccessResult<IEnumerable<LicenseTypeListItem>>(list);
    }
}
