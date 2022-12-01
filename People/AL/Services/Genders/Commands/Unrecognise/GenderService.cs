using Common.ResultPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result> RecogniseGenderAsync(RecogniseGender command)
    {
        return await Task.Run(() => _commandBus.Dispatch(command));
    }
}
