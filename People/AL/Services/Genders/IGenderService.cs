using Common.ResultPattern;
using PersonDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise.StateEvents;
using PersonDomain.AL.Services.Genders.Queries.GetDetails;
using PersonDomain.AL.Services.Genders.Queries.GetDetailsOverTime;
using PersonDomain.AL.Services.Genders.Queries.GetList;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.Genders;
public interface IGenderService
{
    Task<Result<IEnumerable<GenderListItem>>> GetGenderListAsync();
    Task<Result<IEnumerable<GenderOverTime>>> GetGenderDataPointsOverTimeAsync();
    Task<Result<GenderDetails>> GetGenderDetailsAsync(Guid id);
    Task<Result> UnrecogniseGenderAsync(UnrecogniseGender command);
    Task<Result> RecogniseGenderAsync(RecogniseGender command);
    void Handle(RecognisedSucceeded @event); //consider merge the different succeeded and failed into one succeeded and one failed, which can be used by person service too
    void Handle(RecognisedFailed @event);
    void Handle(UnrecognisedSucceeded @event);
    void Handle(UnrecognisedFailed @event);
}
