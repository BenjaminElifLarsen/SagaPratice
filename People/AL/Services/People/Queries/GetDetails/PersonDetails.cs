using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.People.Queries.GetDetails;
public record PersonDetails : BaseReadModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birth { get; set; }
    public int GenderId { get; set; }

    public PersonDetails(int id, string firstName, string lastName, DateOnly birth, int genderId)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Birth = new(birth.Year, birth.Month, birth.Day);
        GenderId = genderId;
    }
}
