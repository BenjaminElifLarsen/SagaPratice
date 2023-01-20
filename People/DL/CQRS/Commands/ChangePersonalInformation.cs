using Common.CQRS.Commands;

namespace PersonDomain.DL.CQRS.Commands;
public sealed class ChangePersonalInformationFromUser : ICommand
{
    public Guid Id { get; set; }
    public ChangeFirstName? FirstName { get; set; }
    public ChangeLastName? LastName { get; set; }
    public ChangeBirth? Birth { get; set; }
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

    internal ChangePersonalInformationFromUser(Guid id, string? firstName, string? lastName, DateOnly? changeBirth, Guid? changeGender) : this()
    {
        Id = id;
        FirstName = firstName is not null ? new() { FirstName = firstName } : null;
        LastName = lastName is not null ? new() { LastName = lastName } : null;
        Birth = changeBirth is not null ? new() { Birth = changeBirth.Value} : null;
        Gender = changeGender is not null ? new() { Gender = changeGender.Value} : null;
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

public record ChangeBirth
{
    public DateOnly Birth { get; set; }
}

public record ChangeGender
{
    public Guid Gender { get; set; }
}
