using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries;
internal class VehicleInformationIdQuery : BaseQuery<VehicleInformation, VehicleInformationIdValidation>
{
    public override Expression<Func<VehicleInformation, VehicleInformationIdValidation>> Map()
    {
        return e => new(e.Id);
    }
}
