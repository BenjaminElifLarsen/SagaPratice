using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services;
public interface IVehicleService
{
    Task<Result<IEnumerable<OperatorListItem>>> GetOperatorListAsync();
}
