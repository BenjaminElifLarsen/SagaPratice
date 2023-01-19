using Common.ProcessManager;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Command;
using PersonDomain.AL.ProcessManagers.Person.Fire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.Hire.StateEvents;
using PersonDomain.AL.ProcessManagers.Person.PersonalInformationChange.StatesEvent;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Services.People;
public sealed partial class PersonService : IPersonService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPersonCommandBus _commandBus;
	private readonly IEnumerable<IProcessManager> _processManagers;
	private Result? _result;

	public PersonService(IUnitOfWork unitOfWork, IPersonCommandBus commandBus, IEnumerable<IProcessManager> processManagers)
	{
        _unitOfWork = unitOfWork;
		_commandBus = commandBus;
		_processManagers = processManagers;
	}

    private void Handler(ProcesserFinished @event) => _result = @event.Result;

    public void Handle(FiredSucceeded @event) { }

    public void Handle(FiredFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(RemovedFromGenderSucceeded @event) => _result = new SuccessResultNoData();

    public void Handle(RemovedFromGenderFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(HiredSucceeded @event) { }

    public void Handle(HiredFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    public void Handle(AddedToGenderSucceeded @event) => _result = new SuccessResultNoData();

    public void Handle(AddedToGenderFailed @event) => _result = new InvalidResultNoData(@event.Errors);

    private bool CanReturnResult => _result is not null;
}
