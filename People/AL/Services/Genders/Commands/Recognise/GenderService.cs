using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result> RecogniseGenderAsync(RecogniseGender command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        while (!CanReturnResult) ;
        //_eventBus.UnregisterHandler<RecognisedSucceeded>(Handle); //not really needed to be called since all references only exist as scoped, not singleton
        //_eventBus.UnregisterHandler<RecognisedFailed>(Handle); //could put void Handle<StateEvent> over in contract and call the service in a middleware to set these up
        return _result;
    }
}
