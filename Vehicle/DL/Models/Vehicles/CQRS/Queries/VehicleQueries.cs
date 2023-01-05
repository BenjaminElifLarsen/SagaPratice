using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries;
internal class VehicleIdQuery : BaseQuery<Vehicle, VehicleId>
{
    public override Expression<Func<Vehicle, VehicleId>> Map()
    {
        return e => new(e.Id);
    }
}
