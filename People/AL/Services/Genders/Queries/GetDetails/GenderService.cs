using Common.ResultPattern;
using PersonDomain.AL.Services.Genders.Queries;
using PersonDomain.AL.Services.Genders.Queries.GetDetails;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;

namespace PersonDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result<GenderDetails>> GetGenderDetailsAsync(Guid id)
    {
        var details = await _unitOfWork.GenderRepository.GetAsync(id, new GenderDetailsQuery());
        var test = (_unitOfWork.GenderEventRepository as GenderEventRepository).TestDeleteLater(id);
        if(details is null)
        {
            return new NotFoundResult<GenderDetails>("Not found.");
        }
        return new SuccessResult<GenderDetails>(details);
    }
}
