using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.AL.Services.Vehicles.Queries.GetDetails;
using VehicleDomain.AL.Services.Vehicles.Queries.GetList;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.AL.Services.Vehicles.Queries;
internal class VehicleListItemQuery : BaseQuery<Vehicle, VehicleListItem>
{
    public override Expression<Func<Vehicle, VehicleListItem>> Map()
    {
        return e => new(e.VehicleId, e.SerielNumber.SerialNumber, e.Operators.Count());
    }
}

internal class VehicleDetailsQuery : BaseQuery<Vehicle, VehicleDetails>
{
    public override Expression<Func<Vehicle, VehicleDetails>> Map()
    {
        return e => new(e.VehicleId, e.ProductionDate, e.DistanceMovedKm, e.InUse, e.SerielNumber.SerialNumber, e.Operators.Select(x => x.Id));
    }
}
