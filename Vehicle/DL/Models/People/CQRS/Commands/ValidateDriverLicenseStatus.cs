using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.CQRS.Commands;
public class ValidateDriverLicenseStatus : ICommand
{
    public int OwnerId { get; private set; }
    public int TypeId { get; private set; }
}

public class ValidatePersonLicenses : ICommand
{ //person id
    public int Id { get; private set; }
}
