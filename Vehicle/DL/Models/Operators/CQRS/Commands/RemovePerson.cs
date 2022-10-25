using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class RemoveOperatorFromSystem : ICommand
{

}

public class RemoveOperatorFromUser : ICommand
{ //errors and checks might need to be handled different, e.g. FromSystem can only remember operator over a specific age

}