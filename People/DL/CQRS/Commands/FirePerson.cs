using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class FirePersonFromUser : ICommand
{
    public int Id { get; set; }
    public DateTime FiredFrom { get; set; }
}
