using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.Genders.Queries.GetDetails;
public record GenderDetails : BaseReadModel
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Object { get; set; }
    public IEnumerable<int> OperatorIds { get; set; }

    public GenderDetails(int id, string subject, string @object, IEnumerable<int> operatorIds)
    {
        Id = id;
        Subject = subject;
        Object = @object;
        OperatorIds = operatorIds;
    }
}
