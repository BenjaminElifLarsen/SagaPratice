using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries;
internal class OperatorIdQuery : BaseQuery<Operator, OperatorIdValidation>
{
    public override Expression<Func<Operator, OperatorIdValidation>> Map()
    {
        return e => new(e.OperatorId);
    }
}
