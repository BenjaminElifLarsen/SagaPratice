using Common.ResultPattern;
using PeopleDomain.AL.Services.Genders.Queries.GetDetails;
using PeopleDomain.AL.Services.Genders.Queries.GetList;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.Genders;
public interface IGenderService
{
    Task<Result<IEnumerable<GenderListItem>>> GetGenderListAsync();
    Task<Result<GenderDetails>> GetGenderDetailsAsync(int id);
    Task<Result> RecogniseGenderAsync(RecogniseGender command);
    Task<Result> UnrecogniseGenderAsync(UnrecogniseGender command);
}
