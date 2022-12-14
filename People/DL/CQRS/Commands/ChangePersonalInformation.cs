using Common.CQRS.Commands;

namespace PersonDomain.DL.CQRS.Commands;
public sealed class ChangePersonalInformationFromUser : ICommand
{
    public Guid Id { get; set; }
    public ChangeFirstName? FirstName { get; set; }
    public ChangeLastName? LastName { get; set; }
    public ChangeBrith? Brith { get; set; }
    public ChangeGender? Gender { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ChangePersonalInformationFromUser()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }
}

public record ChangeFirstName
{
    public string FirstName { get; set; }
}

public record ChangeLastName
{
    public string LastName { get; set; }
}

public record ChangeBrith
{
    public DateOnly Birth { get; set; }
}

public record ChangeGender
{
    public Guid Gender { get; set; }
}
