using Common.CQRS.Commands;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService : IPeopleService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICommandBus _commandBus;

	public PeopleService(IUnitOfWork unitOfWork, ICommandBus commandBus)
	{
        _unitOfWork = unitOfWork;
		_commandBus = commandBus;
	}
}
