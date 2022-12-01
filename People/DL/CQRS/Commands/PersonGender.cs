using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class AddPersonToGender : ICommand //consider a better name
{
    public int PersonId { get; private set; }
    public int GenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal AddPersonToGender(int personId, int genderId, Guid correlationId, Guid causationId)
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
    public int PersonId { get; private set; }
    public int GenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal RemovePersonFromGender(int personId, int genderId, Guid correlationId, Guid causationId)
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
    public int PersonId { get; private set; }
    public int NewGenderId { get; private set; }
    public int OldGenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    internal ChangePersonGender(int personId, int newGenderId, int oldGenderId, Guid correlationId, Guid causationId)
    {
        PersonId = personId;
        NewGenderId = newGenderId;
        OldGenderId = oldGenderId;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}