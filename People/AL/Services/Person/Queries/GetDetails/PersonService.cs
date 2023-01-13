using Common.ResultPattern;
using PersonDomain.AL.Services.People.Queries;
using PersonDomain.AL.Services.People.Queries.GetDetails;

namespace PersonDomain.AL.Services.People;
public partial class PersonService
{
    public async Task<Result<PersonDetails>> GetPersonDetailsAsync(Guid id)
    {
        var details = await _unitOfWork.PersonRepository.GetAsync(id, new PersonDetailsQuery());
        var test = await _unitOfWork.PersonEventRepository.GetForOperationAsync(id);
        if(details is null)
        {
            return new NotFoundResult<PersonDetails>("Not found.");
        }
        return new SuccessResult<PersonDetails>(details);
    }
}
