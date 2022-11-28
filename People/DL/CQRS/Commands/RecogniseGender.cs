using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class RecogniseGender : ICommand
{ //figure out a better name
    public string VerbSubject { get; set; }
    public string VerbObject { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    public RecogniseGender()
    {
        CommandId = Guid.NewGuid();
    }
}
