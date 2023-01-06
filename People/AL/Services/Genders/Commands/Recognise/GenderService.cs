using Common.ResultPattern;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result> RecogniseGenderAsync(RecogniseGender command)
    {
        _eventBus.RegisterHandler<RecognisedSucceeded>(Handle);
        _eventBus.RegisterHandler<RecognisedFailed>(Handle);
        await Task.Run(() => _commandBus.Dispatch(command));
        while (!CanReturnResult) ;
        _eventBus.UnregisterHandler<RecognisedSucceeded>(Handle);
        _eventBus.UnregisterHandler<RecognisedFailed>(Handle);
        return _result;
    }

    private void Handle(RecognisedSucceeded @event) => _result = new SuccessResultNoData();
    private void Handle(RecognisedFailed @event) => _result = new InvalidResultNoData(@event.Errors.ToArray());
}
