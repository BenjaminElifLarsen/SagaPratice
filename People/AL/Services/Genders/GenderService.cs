using PeopleDomain.AL.Busses.Command;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService : IGenderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPeopleCommandBus _commandBus;

    public GenderService(IUnitOfWork unitOfWork, IPeopleCommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }
}
