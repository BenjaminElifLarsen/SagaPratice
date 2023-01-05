using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService
{
    public async Task<Result> RemoveOperatorFromSystemAsync(RemoveOperatorFromSystem command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        throw new NotImplementedException();//return;
    }
}
