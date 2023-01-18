using Common.ResultPattern;
using PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
using PersonDomain.AL.Services.People.Queries.GetDetails;
using PersonDomain.AL.Services.People.Queries.GetList;
using PersonDomain.AL.Services.People.Queries.GetPeoplesGendersOverTime;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.People;
public interface IPersonService
{
    Task<Result<IEnumerable<PersonListItem>>> GetPeopleListAsync();
    Task<Result<IEnumerable<PersonGenderChanges>>> GetGendersOverTimeAsync();
    Task<Result<PersonDetails>> GetPersonDetailsAsync(Guid id);
    Task<Result> HirePersonAsync(HirePersonFromUser command);
    Task<Result> FirePersonAsync(FirePersonFromUser command);
    Task<Result> ChangePersonalInformationAsync(ChangePersonalInformationFromUser command);
    //void Handle(RecognisedSucceeded @event); //these two are related to gender, not person
    //void Handle(RecognisedFailed @event);
    void Handle(FiredSucceeded @event);
    void Handle(FiredFailed @event);
    void Handle(RemovedFromGenderSucceeded @event);
    void Handle(RemovedFromGenderFailed @event);
    void Handle(HiredSucceeded @event);
    void Handle(HiredFailed @event);
    void Handle(AddedToGenderSucceeded @event);
    void Handle(AddedToGenderFailed @event);
}
