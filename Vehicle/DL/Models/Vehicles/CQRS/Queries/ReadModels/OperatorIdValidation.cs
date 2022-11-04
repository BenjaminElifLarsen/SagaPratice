using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;
internal record OperatorIdValidation : BaseReadModel //maybe base read model should be a record
{
    public int Id { get; private set; }
	public OperatorIdValidation(int id)
	{
		Id = id;
	}
}
