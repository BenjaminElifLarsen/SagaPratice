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

public class OperatorDetailsQuery : BaseQuery<Operator, OperatorDetails>
{
    public override Expression<Func<Operator, OperatorDetails>> Map()
    {
        return e => new(e.OperatorId, e.Birth,
            e.Vehicles.Select(x => x.Id),
            e.Licenses.AsQueryable().Select(new OperatorLicenseDetailsQuery().Map()).ToList());
    }
}

internal class OperatorLicenseDetailsQuery : BaseQuery<License, OperatorLicenseDetails>
{
    public override Expression<Func<License, OperatorLicenseDetails>> Map()
    {
        return e => new(e.Arquired, e.LastRenewed, e.Expired, e.Type.Id);
    }
}
