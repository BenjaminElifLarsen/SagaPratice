using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;
internal record VehicleId : BaseReadModel
{
    public Guid Id { get; private set; }
	public VehicleId(Guid id)
	{
		Id = id;
	}
}
