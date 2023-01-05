using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService
{
    public async Task<Result> AddOperatorFromSystemAsync(AddOperatorNoLicenseFromSystem command)
    {
        await Task.Run(() => _commandBus.Dispatch(command));
        throw new NotImplementedException(); //return; //need to return a result, just like the servies that use pms do, but no pm for these yet
    }
}
