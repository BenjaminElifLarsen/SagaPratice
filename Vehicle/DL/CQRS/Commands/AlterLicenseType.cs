using Common.CQRS.Commands;

namespace VehicleDomain.DL.CQRS.Commands;
public class AlterLicenseType : ICommand
{
    public int Id { get; private set; }
    public ChangeType ChangeType { get; private set; }
}

public class ChangeType
{
    public string? Type { get; private set; }
}