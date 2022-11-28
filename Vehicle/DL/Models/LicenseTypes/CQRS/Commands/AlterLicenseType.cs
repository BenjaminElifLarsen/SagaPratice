using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class AlterLicenseType : ICommand
{
    public int Id { get; set; }
    public ChangeType? Type { get; set; }
    public ChangeAgeRequirement? AgeRequirement { get; set; }
    public ChangeRenewPeriod? RenewPeriod { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    public AlterLicenseType()
    {
        CommandId = Guid.NewGuid();
    }

    //public AlterLicenseType(Guid correlationId, Guid causationId)
    //{
    //    CommandId = Guid.NewGuid();
    //    CorrelationId = correlationId;
    //    CausationId = causationId;
    //}
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
