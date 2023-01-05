using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;
internal record VehicleInformationIdValidation : BaseReadModel
{
    public Guid Id { get; private set; }

	public VehicleInformationIdValidation(Guid id)
	{
		Id = id;
	}
}
