using Common.ResultPattern;
using PersonDomain.AL.Services.Genders.Queries;
using PersonDomain.AL.Services.Genders.Queries.GetDetailsOverTime;

namespace PersonDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result<IEnumerable<GenderOverTime>>> GetGenderDataPointsOverTimeAsync()
    {
        var list = await _unitOfWork.GenderEventRepository.AllAsync(new GenderOverTimeView());
        return new SuccessResult<IEnumerable<GenderOverTime>>(list);
    }
}
