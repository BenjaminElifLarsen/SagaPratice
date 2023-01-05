using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public sealed class AddPersonToGender : ICommand //consider a better name
{
    public Guid PersonId { get; private set; }
    public Guid GenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal AddPersonToGender(Guid personId, Guid genderId, Guid correlationId, Guid causationId)
    {
        PersonId = personId;
        GenderId = genderId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}

public class RemovePersonFromGender : ICommand
{
    public Guid PersonId { get; private set; }
    public Guid GenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal RemovePersonFromGender(Guid personId, Guid genderId, Guid correlationId, Guid causationId)
    {
        PersonId = personId;
        GenderId = genderId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}

public class ChangePersonGender : ICommand
{
    public Guid PersonId { get; private set; }
    public Guid NewGenderId { get; private set; }
    public Guid OldGenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal ChangePersonGender(Guid personId, Guid newGenderId, Guid oldGenderId, Guid correlationId, Guid causationId)
    {
        PersonId = personId;
        NewGenderId = newGenderId;
        OldGenderId = oldGenderId;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}