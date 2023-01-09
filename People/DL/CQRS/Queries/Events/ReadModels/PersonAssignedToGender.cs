using Common.CQRS.Queries;

namespace PersonDomain.DL.CQRS.Queries.Events.ReadModels;
internal sealed record PersonAssignedToGender : BaseReadModel
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public PersonAssignedToGender(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}
