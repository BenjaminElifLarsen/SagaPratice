using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Queries;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services;
public partial class VehicleService
{
    public async Task<Result<IEnumerable<OperatorListItem>>> GetOperatorListAsync()
    {
        var list = await _operatorRepository.AllAsync(new OperatorListItemQuery());
        return new SuccessResult<IEnumerable<OperatorListItem>>(list);
    }
}
