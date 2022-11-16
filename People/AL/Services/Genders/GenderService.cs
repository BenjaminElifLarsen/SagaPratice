using PeopleDomain.AL.Handlers.Command;
using PeopleDomain.IPL.Repositories;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService : IGenderService
{
    private readonly IPeopleCommandHandler _peopleCommandHandler;
    private readonly IGenderRepository _genderRepository;

    public GenderService(IPeopleCommandHandler peopleCommandHandler, IGenderRepository genderRepository)
    {
        _peopleCommandHandler = peopleCommandHandler;
        _genderRepository = genderRepository;
    }
}
