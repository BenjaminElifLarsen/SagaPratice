using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class HirePersonFromUser : ICommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birth { get; set; }
    public int Gender { get; set; }
}
