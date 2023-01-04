using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result> RecogniseGenderAsync(RecogniseGender command)
    {
        throw new NotImplementedException();
        //var processManager = _processManagers.SingleOrDefault(x => x is IRecogniseProcessManager);
        //if(processManager is not null)
        //{
        //    processManager.SetUp(command.CommandId);
        //    processManager.RegistrateHandler(Handler);
        //}
        //await Task.Run(() => _commandBus.Dispatch(command));
        //while (!CanReturnResult) ;
        //return _result;
    }
}
