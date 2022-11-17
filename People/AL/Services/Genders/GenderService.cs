using Common.CQRS.Commands;
using PeopleDomain.IPL.Services;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService : IGenderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandBus _commandBus;

    public GenderService(IUnitOfWork unitOfWork, ICommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }
}
