using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PeopleDomain.AL.Services.Genders.Queries.GetDetails;
using PeopleDomain.AL.Services.Genders.Queries.GetList;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.Genders;
public interface IGenderService
{
    Task<Result<IEnumerable<GenderListItem>>> GetGenderListAsync();
    Task<Result<GenderDetails>> GetGenderDetailsAsync(Guid id);
    
    Task<Result> UnrecogniseGenderAsync(UnrecogniseGender command);


    Task<Result> RecogniseGenderAsync(RecogniseGender command);
    void Handle(RecognisedSucceeded @event);
    void Handle(RecognisedFailed @event);
}
