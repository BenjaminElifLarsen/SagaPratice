using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class AlterLicenseType : ICommand
{
    public int Id { get; set; }
    public ChangeType? Type { get; set; }
    public ChangeAgeRequirement? AgeRequirement { get; set; }
    public ChangeRenewPeriod? RenewPeriod { get; set; }
}

public class ChangeType
{
    public string Type { get; set; }
}

public class ChangeAgeRequirement
{
    public byte AgeRequirement { get; set; }
}

public class ChangeRenewPeriod
{
    public byte RenewPeriod { get; set; }
}
