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
        }
        return await Task.Run(() => _commandBus.Dispatch(command));
    }
}
