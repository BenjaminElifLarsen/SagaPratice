using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Command;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Services.People;
public sealed partial class PersonService : IPersonService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPersonCommandBus _commandBus;
	private readonly IEnumerable<IProcessManager> _processManagers; //maybe have an extra interface to split up process mangers between the different modules.
	private Result? _result;

	public PersonService(IUnitOfWork unitOfWork, IPersonCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
	{
        _unitOfWork = unitOfWork;
		_commandBus = commandBus;
		_processManagers = processManagers;
	}

    private void Handler(ProcesserFinished @event) => _result = @event.Result;
    private bool CanReturnResult => _result is not null;
    //public void Handle(RecognisedSucceeded @event) => _result = new SuccessResultNoData(); //these two are related to gender
    //public void Handle(RecognisedFailed @event) => _result = new InvalidResultNoData(@event.Errors.ToArray());
}
