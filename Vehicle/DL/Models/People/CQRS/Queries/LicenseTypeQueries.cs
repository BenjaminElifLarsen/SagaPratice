using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.People.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.People.CQRS.Queries;
internal class LicenseTypeAgeQuery : BaseQuery<LicenseType, LicenseTypeAgeValidation>
{
    public override Expression<Func<LicenseType, LicenseTypeAgeValidation>> Map()
    {
        return e => new(e.AgeRequirementInYears);
    }
}

internal class LincenseTypeIdQuery : BaseQuery<LicenseType, LicenseTypeIdValidation>
{
    public override Expression<Func<LicenseType, LicenseTypeIdValidation>> Map()
    {
        return e => new(e.LicenseTypeId);
    }
}
