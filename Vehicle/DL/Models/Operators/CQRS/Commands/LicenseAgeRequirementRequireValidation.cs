using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class LicenseAgeRequirementRequireValidation : ICommand
{
    public Guid CommandId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }
    public Guid OperatorId { get; private set; }
    public Guid LicenseTypeId { get; private set; }
    public byte NewAgeRequirement { get; private set; }

    public LicenseAgeRequirementRequireValidation(Guid operatorId, Guid licenseTypeId, byte newAgeRequirement, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
        NewAgeRequirement = newAgeRequirement;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}