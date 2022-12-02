using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Person.Hire;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result> HirePersonAsync(HirePersonFromUser command)
    {
        var processManager = _processManagers.SingleOrDefault(x => x is IHireProcessManager);
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
