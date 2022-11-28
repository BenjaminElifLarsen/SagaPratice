using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class UnrecogniseGender : ICommand
{
    public int Id { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    public UnrecogniseGender()
    {
        CommandId = Guid.NewGuid();
    }
}
