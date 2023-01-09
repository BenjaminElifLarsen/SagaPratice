using Common.CQRS.Queries;

namespace PersonDomain.DL.CQRS.Queries.Events.ReadModels;
internal sealed record GenderSubject : BaseReadModel
{
    public string Subject { get; private set; }

	public GenderSubject(string subject)
	{
		Subject = subject;
	}
}
