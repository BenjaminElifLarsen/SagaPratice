using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService : IGenderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPeopleCommandBus _commandBus;
    private readonly IEnumerable<IProcessManager> _processManagers;
    private Result? _result;

    public GenderService(IUnitOfWork unitOfWork, IPeopleCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
        _processManagers = processManagers;
    }

    private void Handler(ProcesserFinished @event) => _result = @event.Result;
    private bool CanReturnResult => _result is not null;
}
