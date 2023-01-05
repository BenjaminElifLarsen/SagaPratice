using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.Genders.Queries.GetDetails;
public record GenderDetails : BaseReadModel
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Object { get; set; }
    public IEnumerable<Guid> PersonIds { get; set; }

    public GenderDetails(Guid id, string subject, string @object, IEnumerable<Guid> personIds)
    {
        Id = id;
        Subject = subject;
        Object = @object;
        PersonIds = personIds;
    }
}
