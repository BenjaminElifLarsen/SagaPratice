using Common.ResultPattern;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result> UnrecogniseGenderAsync(UnrecogniseGender command)
    {
        var processManager = _processManagers.SingleOrDefault(x => x is IUnrecogniseProcessManager);
        if(processManager is not null)
        {
            processManager.SetUp(command.CommandId);
            processManager.RegistrateHandler(Handler);
        }
        await Task.Run(() => _commandBus.Dispatch(command));
        while (!CanReturnResult) ;
        return _result;
    }
}
