using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class HirePersonFromUser : ICommand
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateOnly Birth { get; private set; }
    public int Gender { get; private set; }
}
