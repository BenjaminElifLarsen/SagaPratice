using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class HirePersonFromUser : ICommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birth { get; set; }
    public int Gender { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    public HirePersonFromUser()
    {
        CommandId = Guid.NewGuid();
    }
}
