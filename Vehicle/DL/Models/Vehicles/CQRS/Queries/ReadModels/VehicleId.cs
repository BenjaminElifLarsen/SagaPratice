using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;
internal record VehicleId : BaseReadModel
{
    public int Id { get; private set; }
	public VehicleId(int id)
	{
		Id = id;
	}
}
