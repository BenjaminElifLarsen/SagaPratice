using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.AL.Services.LicenseTypes.Queries.GetDetails;
using VehicleDomain.AL.Services.LicenseTypes.Queries.GetList;
using VehicleDomain.DL.Models.LicenseTypes;

namespace VehicleDomain.AL.Services.LicenseTypes.Queries;
internal class LicenseTypeListItemQuery : BaseQuery<LicenseType, LicenseTypeListItem>
{
    public override Expression<Func<LicenseType, LicenseTypeListItem>> Map()
    {
        return e => new(e.LicenseTypeId, e.Type, e.RenewPeriodInYears, e.AgeRequirementInYears, e.Operators.Count(), e.VehicleInformations.Count());
    }
}

internal class LicenseTypeDetailsQuery : BaseQuery<LicenseType, LicenseTypeDetails>
{
    public override Expression<Func<LicenseType, LicenseTypeDetails>> Map()
    {
        return e => new(e.LicenseTypeId, e.Type, e.RenewPeriodInYears, e.AgeRequirementInYears, new(e.CanBeIssuedFrom.Year, e.CanBeIssuedFrom.Month, e.CanBeIssuedFrom.Day),
            e.Operators.Select(x => x.Id), e.VehicleInformations.Select(x => x.Id));
    }
}
