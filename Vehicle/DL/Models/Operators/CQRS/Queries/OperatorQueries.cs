using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries;
public class OperatorListItemQuery : BaseQuery<Operator, OperatorListItem>
{
    public override Expression<Func<Operator, OperatorListItem>> Map()
    {
        return e => new(e.OperatorId,
            e.Birth,
            e.Licenses.Count(),
            e.Vehicles.Count());
    }
}
