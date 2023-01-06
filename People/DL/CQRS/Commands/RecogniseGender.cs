using Common.CQRS.Commands;

namespace PersonDomain.DL.CQRS.Commands;
public sealed class RecogniseGender : ICommand
{ //figure out a better name
    public string VerbSubject { get; set; }
    public string VerbObject { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public RecogniseGender()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    internal RecogniseGender(string verbSubject, string verbObject) : this()
    {
        VerbSubject = verbSubject;
        VerbObject = verbObject;
    }
}
