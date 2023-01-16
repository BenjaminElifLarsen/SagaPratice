using Common.ResultPattern;
using PersonDomain.AL.Services.People.Queries;
using PersonDomain.AL.Services.People.Queries.GetList;

namespace PersonDomain.AL.Services.People;
public partial class PersonService
{
    public async Task<Result<IEnumerable<PersonListItem>>> GetPeopleListAsync()
    {
        var list = await _unitOfWork.PersonRepository.AllAsync(new PersonListItemQuery());
        return new SuccessResult<IEnumerable<PersonListItem>>(list);
    }
}
