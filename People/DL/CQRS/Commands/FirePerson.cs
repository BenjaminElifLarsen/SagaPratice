using Common.CQRS.Commands;

namespace PersonDomain.DL.CQRS.Commands;
public sealed class FirePersonFromUser : ICommand
{
    public Guid Id { get; set; }
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
