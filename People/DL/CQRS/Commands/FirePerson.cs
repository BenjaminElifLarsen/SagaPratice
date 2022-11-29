using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class FirePersonFromUser : ICommand
{
    public int Id { get; set; }
    public DateTime FiredFrom { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public FirePersonFromUser()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}
