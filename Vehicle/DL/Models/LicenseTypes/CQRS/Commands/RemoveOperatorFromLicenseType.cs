using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class RemoveOperatorFromLicenseType : ICommand
{
    public Guid OperatorId { get; private set; }
    public Guid LicenseTypeId { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public RemoveOperatorFromLicenseType(Guid operatorId, Guid licenseTypeId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}
