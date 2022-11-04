using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.People.Queries.GetList;
public record PersonListItem : BaseReadModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int GenderId { get; set; }
    public DateTime Birth { get; set; }

    public PersonListItem(int id, string fullName, DateOnly birth, int genderId)
    {
        Id = id;
        FullName = fullName;
        GenderId = genderId;
        Birth = new(birth.Year, birth.Month, birth.Day);
    }
}
