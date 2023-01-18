using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.People;
public partial class PersonService
{
    public async Task<Result> HirePersonAsync(HirePersonFromUser command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        while (!CanReturnResult) ;
        return _result;
    }
}
