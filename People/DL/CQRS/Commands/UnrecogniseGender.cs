using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class UnrecogniseGender : ICommand
{
    public int Id { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public UnrecogniseGender()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}
