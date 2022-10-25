
using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries;
internal class LicenseTypeForVehicleInformationValidationQuery : BaseQuery<LicenseType, LicenseTypeValidation>
{
    public override Expression<Func<LicenseType, LicenseTypeValidation>> Map()
    {
        return e => new(e.LicenseTypeId);
    }
}
