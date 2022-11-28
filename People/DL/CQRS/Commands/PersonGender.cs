using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class AddPersonToGender : ICommand //consider a better name
{
    public int PersonId { get; private set; }
    public int GenderId { get; private set; }

    public Guid CommandId => Guid.NewGuid();

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    internal AddPersonToGender(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}

public class RemovePersonFromGender : ICommand
{
    public int PersonId { get; private set; }
    public int GenderId { get; private set; }

    public Guid CommandId => Guid.NewGuid();

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    internal RemovePersonFromGender(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}

public class ChangePersonGender : ICommand
{
    public int PersonId { get; private set; }
    public int NewGenderId { get; private set; }
    public int OldGenderId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    internal ChangePersonGender(int personId, int newGenderId, int oldGenderId)
    {
        PersonId = personId;
        NewGenderId = newGenderId;
        OldGenderId = oldGenderId;
        CommandId = Guid.NewGuid();
    }
}