using Common.ResultPattern;
using PeopleDomain.AL.Services.People.Queries;
using PeopleDomain.AL.Services.People.Queries.GetList;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result<IEnumerable<PersonListItem>>> GetPeopleListAsync()
    {
        var list = await _unitOfWork.PersonRepository.AllAsync(new PersonListItemQuery());
        return new SuccessResult<IEnumerable<PersonListItem>>(list);
    }
}
