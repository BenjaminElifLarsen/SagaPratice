using Common.ResultPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;

namespace VehicleDomain.AL.Services.Operators;
public interface IOperatorService
{
    Task<Result<IEnumerable<OperatorListItem>>> GetOperatorListAsync();
    Task<Result<OperatorDetails>> GetOperatorDetailsAsync(int id); //ids could be used to search in other aggregates via requests or could take the ids and search the other aggregate roots and then combine and send of
    Task<Result> AddOperatorFromSystemAsync(AddOperatorNoLicenseFromSystem command);
    Task<Result> RemoveOperatorFromSystemAsync(RemoveOperatorFromSystem command);
}
