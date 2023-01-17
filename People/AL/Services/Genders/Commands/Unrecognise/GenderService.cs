using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result> UnrecogniseGenderAsync(UnrecogniseGender command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        while (!CanReturnResult) ;
        return _result;
    }
}
