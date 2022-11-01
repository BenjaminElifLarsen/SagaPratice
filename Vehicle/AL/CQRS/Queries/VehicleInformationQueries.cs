using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.AL.CQRS.Queries.ReadModels;
using VehicleDomain.DL.Models.VehicleInformations;

namespace VehicleDomain.AL.CQRS.Queries;
internal class VehicleInformationListItemQuery : BaseQuery<VehicleInformation, VehicleInformationListItem>
{
    public override Expression<Func<VehicleInformation, VehicleInformationListItem>> Map()
    {
        return e => new(e.VehicleInformationId, e.Name);
    }
}
