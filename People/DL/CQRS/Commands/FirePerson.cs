using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class FirePersonFromUser : ICommand
{
    public int Id { get; private set; }
    public DateOnly FiredFrom { get; private set; }
}
