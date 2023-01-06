using Common.ResultPattern;
using PersonDomain.AL.ProcessManagers.Person.Fire;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.People;
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
