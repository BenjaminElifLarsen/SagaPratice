using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class ValidateOperatorLicenseStatus : ICommand
{
    public int OperatorId { get; set; }
    public int TypeId { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ValidateOperatorLicenseStatus()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    public ValidateOperatorLicenseStatus(int operatorId, int typeId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        TypeId = typeId;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}

//public class ValidateOperatorLicenses : ICommand
//{ //person id
//    public int OperatorId { get; private set; }
//}


public class ValidateLicenseAgeRequirementBecauseChange : ICommand
{
    public int LicenseTypeId { get; private set; }
    public byte AgeRequirement { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ValidateLicenseAgeRequirementBecauseChange(int licenseTypeId, byte ageRequirement, IEnumerable<int> operatorIds, Guid correlationId, Guid causationId)
    {
        LicenseTypeId = licenseTypeId;
        AgeRequirement = ageRequirement;
        OperatorIds = operatorIds;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}

public class ValidateLicenseRenewPeriodBecauseChange : ICommand
{
    public int LicenseTypeId { get; private set; }
    public byte RenewPeriod { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ValidateLicenseRenewPeriodBecauseChange(int licenseTypeId, byte renewPeriod, IEnumerable<int> operatorIds, Guid correlationId, Guid causationId)
    {
        LicenseTypeId = licenseTypeId;
        RenewPeriod = renewPeriod;
        OperatorIds = operatorIds;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}