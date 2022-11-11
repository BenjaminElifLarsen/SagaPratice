using PeopleDomain.AL.CQRS.Commands.Handlers;
using PeopleDomain.DL.Events.Domain;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService : IPeopleService
{
	private readonly IPeopleCommandHandler _peopleCommandHandler;
	private readonly IPersonRepository _personRepository;
	private readonly IPersonEventPublisher _personEventPublisher;

	public PeopleService(IPersonRepository personRepository, IPeopleCommandHandler peopleCommandHandler, IPersonEventPublisher personEventPublisher)
	{
		_personRepository = personRepository;
		_peopleCommandHandler = peopleCommandHandler;
		_personEventPublisher = personEventPublisher;
	}
}
