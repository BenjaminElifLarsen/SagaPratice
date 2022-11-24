using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Queries;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services.Operators;
public partial class OperatorService
{
    public async Task<Result<OperatorDetails>> GetOperatorDetailsAsync(int id)
    {
        var details = await Task.Run(() => _unitOfWork.OperatorRepository.GetAsync(id, new OperatorDetailsQuery()));
        if(details is null)
        {
            return new NotFoundResult<OperatorDetails>("No operator found with the given id.");
        }
        return new SuccessResult<OperatorDetails>(details);
    }
}
