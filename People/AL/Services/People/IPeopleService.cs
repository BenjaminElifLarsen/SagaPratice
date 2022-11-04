using Common.ResultPattern;
using PeopleDomain.AL.Services.People.Queries.GetDetails;
using PeopleDomain.AL.Services.People.Queries.GetList;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.People;
public interface IPeopleService
{
    Task<Result<IEnumerable<PersonListItem>>> GetPeopleListAsync();
    Task<Result<PersonDetails>> GetPersonDetailsAsync(int id);
    Task<Result> HirePersonAsync(HirePersonFromUser command);
    Task<Result> FirePersonAsync(FirePersonFromUser command);
}
