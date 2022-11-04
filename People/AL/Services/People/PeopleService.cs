using PeopleDomain.DL.CQRS.Commands.Handlers;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.AL.Services.People;
public partial class PeopleService : IPeopleService
{
	private readonly IPeopleCommandHandler _peopleCommandHandler;
	private readonly IPersonRepository _personRepository;

	public PeopleService(IPersonRepository personRepository, IPeopleCommandHandler peopleCommandHandler)
	{
		_personRepository = personRepository;
		_peopleCommandHandler = peopleCommandHandler;
	}
}
