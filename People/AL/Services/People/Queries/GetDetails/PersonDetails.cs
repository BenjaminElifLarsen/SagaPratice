using Common.CQRS.Queries;

namespace PeopleDomain.AL.Services.People.Queries.GetDetails;
public record PersonDetails : BaseReadModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birth { get; set; }
    public Guid GenderId { get; set; }

    public PersonDetails(Guid id, string firstName, string lastName, DateOnly birth, Guid genderId)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Birth = new(birth.Year, birth.Month, birth.Day);
        GenderId = genderId;
    }
}
