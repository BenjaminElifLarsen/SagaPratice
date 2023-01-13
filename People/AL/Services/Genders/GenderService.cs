using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.Busses.Event;
using PersonDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Services.Genders;
public sealed partial class GenderService : IGenderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonCommandBus _commandBus;
    private readonly IEnumerable<IProcessManager> _processManagers;
    private Result? _result;

    public GenderService(IUnitOfWork unitOfWork, IPersonCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
        _processManagers = processManagers;
    }

    private void Handler(ProcesserFinished @event) => _result = @event.Result;
    private bool CanReturnResult => _result is not null;
    public void Handle(RecognisedSucceeded @event) => _result = new SuccessResultNoData();
    public void Handle(RecognisedFailed @event) => _result = new InvalidResultNoData(@event.Errors.ToArray());
}
