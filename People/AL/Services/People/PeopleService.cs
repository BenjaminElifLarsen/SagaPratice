using Common.Events.Domain;
using PeopleDomain.AL.Handlers.Command;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService : IPeopleService
{
	private readonly IPeopleCommandHandler _peopleCommandHandler;
	private readonly IPersonRepository _personRepository;
	private readonly IDomainEventBus _personEventPublisher;

	public PeopleService(IPersonRepository personRepository, IPeopleCommandHandler peopleCommandHandler, IDomainEventBus personEventPublisher)
	{
		_personRepository = personRepository;
		_peopleCommandHandler = peopleCommandHandler;
		_personEventPublisher = personEventPublisher;
	}
}
