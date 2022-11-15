using Common.CQRS.Commands;

namespace PeopleDomain.DL.CQRS.Commands;
public class UnrecogniseGender : ICommand
{
    public int Id { get; set; }
}
