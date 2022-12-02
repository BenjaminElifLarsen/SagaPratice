using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Person.Fire;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result> FirePersonAsync(FirePersonFromUser command)
    {
        var processManager = _processManagers.SingleOrDefault(x => x is IFireProcessManager);
        if(processManager is not null)
        {
            processManager.SetUp(command.CommandId);
            processManager.RegistrateHandler(Handler);
        } //should return the task result if pm is null
        /*var result = */await Task.Run(() => _commandBus.Dispatch(command));
        //if (result is InvalidResultNoData) return result;
        while (!CanReturnResult) ;
        return _result;
    }
}
