using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Person.PersonalInformationChange;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService
{
    public async Task<Result> ChangePersonalInformationAsync(ChangePersonalInformationFromUser command)
    {
        var processManager = _processManagers.SingleOrDefault(x => x is IPersonalInformationChangeProcessManager);
        if(processManager is not null)
        {
            processManager.SetUp(command.CommandId);
            processManager.SetCallback(Callback);
        }
        var result = await Task.Run(() => _commandBus.Dispatch(command));
        if(result is InvalidResultNoData) return result; //temp. solution
        while (!CanReturnResult) ; //this will not work if the entity was not found, redesign.
        return _result; //the problem lies in the fact that domain events are placed on aggregate roots and if none is found there is no place to place the event.
    } //could return the result of the task, at leat for now, while figuring out how to solve the problem
}
//{
//  "id": 1,
//  "gender": {
//    "gender": 1755192844
//  }
//}