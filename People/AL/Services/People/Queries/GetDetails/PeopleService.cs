using Common.ResultPattern;
using PeopleDomain.AL.Services.People.Queries;
using PeopleDomain.AL.Services.People.Queries.GetDetails;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result<PersonDetails>> GetPersonDetailsAsync(int id)
    {
        var details = await _unitOfWork.PersonRepository.GetAsync(id, new PersonDetailsQuery());
        if(details is null)
        {
            return new NotFoundResult<PersonDetails>("Not found.");
        }
        return new SuccessResult<PersonDetails>(details);
    }
}
