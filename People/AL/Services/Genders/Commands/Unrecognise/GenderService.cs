using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Gender.Unrecognise;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.Genders;
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
