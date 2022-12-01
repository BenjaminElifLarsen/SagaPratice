using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService
{
    public async Task<Result> RemoveOperatorFromSystemAsync(RemoveOperatorFromSystem command)
    {
        return await Task.Run(() => _commandBus.Dispatch(command));
    }
}
