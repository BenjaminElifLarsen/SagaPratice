using Common.CQRS.Queries;

namespace PeopleDomain.DL.CQRS.Queries.ReadModels;
internal sealed record GenderIdValidation : BaseReadModel
{
    public Guid Id { get; private set; }

	public GenderIdValidation(Guid id)
	{
		Id = id;
	}
}
