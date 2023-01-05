using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;
internal record OperatorIdValidation : BaseReadModel //maybe base read model should be a record
{
    public Guid Id { get; private set; }
	public OperatorIdValidation(Guid id)
	{
		Id = id;
	}
}
