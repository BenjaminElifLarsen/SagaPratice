using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.People.Queries.GetList;
public record PersonListItem : BaseReadModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public Guid GenderId { get; set; }
    public DateTime Birth { get; set; }

    public PersonListItem(Guid id, string fullName, DateOnly birth, Guid genderId)
    {
        Id = id;
        FullName = fullName;
        GenderId = genderId;
        Birth = new(birth.Year, birth.Month, birth.Day);
    }
}
