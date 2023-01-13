using Common.ResultPattern;
using PersonDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PersonDomain.AL.Services.People.Queries.GetDetails;
using PersonDomain.AL.Services.People.Queries.GetList;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.People;
public interface IPersonService
{
    Task<Result<IEnumerable<PersonListItem>>> GetPeopleListAsync();
    Task<Result<PersonDetails>> GetPersonDetailsAsync(Guid id);
    Task<Result> HirePersonAsync(HirePersonFromUser command);
    Task<Result> FirePersonAsync(FirePersonFromUser command);
    Task<Result> ChangePersonalInformationAsync(ChangePersonalInformationFromUser command);
    void Handle(RecognisedSucceeded @event);
    void Handle(RecognisedFailed @event);
}
