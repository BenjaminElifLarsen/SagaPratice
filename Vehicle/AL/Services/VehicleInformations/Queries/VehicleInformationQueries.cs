using Common.CQRS.Queries;
using System.Linq.Expressions;
using VehicleDomain.AL.Services.VehicleInformations.Queries.GetDetails;
using VehicleDomain.AL.Services.VehicleInformations.Queries.GetList;
using VehicleDomain.DL.Models.VehicleInformations;

namespace VehicleDomain.AL.Services.VehicleInformations.Queries;
internal class VehicleInformationListItemQuery : BaseQuery<VehicleInformation, VehicleInformationListItem>
{
    public override Expression<Func<VehicleInformation, VehicleInformationListItem>> Map()
    {
        return e => new(e.VehicleInformationId, e.Name, e.Vehicles.Count());
    }
}

internal class VehicleInformationDetailsQuery : BaseQuery<VehicleInformation, VehicleInformationDetails>
{
    public override Expression<Func<VehicleInformation, VehicleInformationDetails>> Map()
    {
        return e => new(e.VehicleInformationId, e.Name, e.MaxWheelAmount, e.LicenseTypeRequired.Id, e.Vehicles.Select(x => x.Id));
    }
}
