using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class ValidateOperatorLicenseStatus : ICommand
{
    public Guid OperatorId { get; set; }
    public Guid TypeId { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ValidateOperatorLicenseStatus()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    public ValidateOperatorLicenseStatus(Guid operatorId, Guid typeId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        TypeId = typeId;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}

public class ValidateLicenseAgeRequirementBecauseChange : ICommand
{
    public Guid LicenseTypeId { get; private set; }
    public byte AgeRequirement { get; private set; }
    public IEnumerable<Guid> OperatorIds { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ValidateLicenseAgeRequirementBecauseChange(Guid licenseTypeId, byte ageRequirement, IEnumerable<Guid> operatorIds, Guid correlationId, Guid causationId)
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
    public Guid LicenseTypeId { get; private set; }
    public byte RenewPeriod { get; private set; }
    public IEnumerable<Guid> OperatorIds { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public ValidateLicenseRenewPeriodBecauseChange(Guid licenseTypeId, byte renewPeriod, IEnumerable<Guid> operatorIds, Guid correlationId, Guid causationId)
    {
        LicenseTypeId = licenseTypeId;
        RenewPeriod = renewPeriod;
        OperatorIds = operatorIds;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}