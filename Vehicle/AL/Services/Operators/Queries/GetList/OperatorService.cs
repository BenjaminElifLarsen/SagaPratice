using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Queries;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService
{
    public async Task<Result<IEnumerable<OperatorListItem>>> GetOperatorListAsync()
    {
        var list = await Task.Run(() => _unitOfWork.OperatorRepository.AllAsync(new OperatorListItemQuery()));
        return new SuccessResult<IEnumerable<OperatorListItem>>(list);
    }
}
