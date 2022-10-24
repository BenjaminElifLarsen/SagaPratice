using Common.CQRS.Commands;

namespace VehicleDomain.DL.CQRS.Commands;
public class RemovePersonFromSystem : ICommand
{
}

public class RemovePersonFromUser : ICommand
{ //errors and checks might need to be handled different, e.g. FromSystem can only remember people over a specific age
}

