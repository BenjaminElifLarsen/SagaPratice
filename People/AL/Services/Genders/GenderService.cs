using Common.Events.Save;
using Common.ResultPattern;
using PersonDomain.AL.Busses.Command;
using PersonDomain.IPL.Services;

namespace PersonDomain.AL.Services.Genders;
public sealed partial class GenderService : IGenderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonCommandBus _commandBus;
    private Result? _result;
    private bool CanReturnResult => _result is not null;

    public GenderService(IUnitOfWork unitOfWork, IPersonCommandBus commandBus)
    {
        _unitOfWork = unitOfWork;
        _commandBus = commandBus;
    }

    public void Handle(ProcessingSucceeded @evnet) => _result = new SuccessResultNoData();

    public void Handle(ProcessingFailed @event) => _result = new InvalidResultNoData(@event.Errors);
}
