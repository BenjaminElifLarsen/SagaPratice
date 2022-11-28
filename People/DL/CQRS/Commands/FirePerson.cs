using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class FirePersonFromUser : ICommand
{
    public int Id { get; set; }
    public DateTime FiredFrom { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    public FirePersonFromUser()
    {
        CommandId = Guid.NewGuid();
    }
}
