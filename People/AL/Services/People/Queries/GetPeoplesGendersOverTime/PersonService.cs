using Common.ResultPattern;
using PersonDomain.AL.Services.People.Queries;
using PersonDomain.AL.Services.People.Queries.GetPeoplesGendersOverTime;

namespace PersonDomain.AL.Services.People;
public partial class PersonService
{
    public async Task<Result<IEnumerable<PersonGenderChanges>>> GetGendersOverTimeAsync()
    {
        var list = await _unitOfWork.PersonEventRepository.AllAsync(new PersonGenderChangesView());
        return new SuccessResult<IEnumerable<PersonGenderChanges>>(list);
    }
}
