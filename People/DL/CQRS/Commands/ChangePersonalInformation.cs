using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class ChangePersonalInformationFromUser : ICommand
{
    public int Id { get; private set; }
    public ChangeFirstName FirstName { get; private set; }
    public ChangeLastName LastName { get; private set; }
    public ChangeBrith Brith { get; private set; }
    public ChangeGender Gender { get; private set; }
}

public record ChangeFirstName
{
    public string FirstName { get; private set; }
}

public record ChangeLastName
{
    public string LastName { get; private set; }
}

public record ChangeBrith
{
    public DateOnly Birth { get; private set; }
}

public record ChangeGender
{
    public int Gender { get; private set; }
}
