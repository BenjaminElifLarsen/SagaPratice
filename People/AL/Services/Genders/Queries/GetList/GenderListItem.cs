using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.Genders.Queries.GetList;
public record GenderListItem : BaseReadModel
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Object { get; set; }
    public int NumberOfPeople { get; set; }

    public GenderListItem(int id, string subject, string @object, int numberOfPeople)
    {
        Id = id;
        Subject = subject;
        Object = @object;
        NumberOfPeople = numberOfPeople;
    }
}
