using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Command;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Services.People;
public sealed partial class PersonService : IPersonService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPersonCommandBus _commandBus;
	private readonly IEnumerable<IProcessManager> _processManagers;
	private Result? _result;
    private bool _genderChangeDone, _genderChangeRequested = false;
    private bool _informationChangeDone = false;

	public PersonService(IUnitOfWork unitOfWork, IPersonCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
	{
        _unitOfWork = unitOfWork;
		_commandBus = commandBus;
		_processManagers = processManagers;
	}


    private bool CanReturnResult => _result is not null;

}
