using Common.ResultPattern;
using PersonDomain.AL.Services.Genders.Queries;
using PersonDomain.AL.Services.Genders.Queries.GetList;

namespace PersonDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result<IEnumerable<GenderListItem>>> GetGenderListAsync()
    {
        var list = await _unitOfWork.GenderRepository.AllAsync(new GenderListItemQuery());
        return new SuccessResult<IEnumerable<GenderListItem>>(list);
    }
}
