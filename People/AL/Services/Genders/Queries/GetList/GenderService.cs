using Common.ResultPattern;
using PeopleDomain.AL.Services.Genders.Queries;
using PeopleDomain.AL.Services.Genders.Queries.GetList;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result<IEnumerable<GenderListItem>>> GetGenderListAsync()
    {
        var list = await _unitOfWork.GenderRepository.AllAsync(new GenderListItemQuery());
        return new SuccessResult<IEnumerable<GenderListItem>>(list);
    }
}
