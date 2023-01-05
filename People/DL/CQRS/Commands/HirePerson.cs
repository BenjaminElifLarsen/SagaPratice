using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public sealed class HirePersonFromUser : ICommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birth { get; set; }
    public Guid Gender { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public HirePersonFromUser()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    internal HirePersonFromUser(string firstName, string lastName, Guid gender, DateTime birth) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Birth = birth;
    }
}
