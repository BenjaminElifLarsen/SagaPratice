using Common.CQRS.Commands;
using Common.ProcessManager;
using Common.ResultPattern;
using PeopleDomain.AL.Busses.Command;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService : IPeopleService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPeopleCommandBus _commandBus;
	private readonly IEnumerable<IProcessManager> _processManagers; //maybe have an extra interface to split up process mangers between the different modules.
	private Result? _result;

	public PeopleService(IUnitOfWork unitOfWork, IPeopleCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
	{
        _unitOfWork = unitOfWork;
		_commandBus = commandBus;
		_processManagers = processManagers;
	}

    private void Handler(ProcesserFinished @event) => _result = @event.Result;
    private bool CanReturnResult => _result is not null;
}
