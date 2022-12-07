using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
public class RemoveOperatorIfSpecificLicenseType : ICommand
{
    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }
    public int OperatorId { get; private set; }
    public int VehicleId { get; private set; }
    public int LicenseTypeId { get; private set; }

    public RemoveOperatorIfSpecificLicenseType(int operatorId, int vehicleId, int licenseTypeId, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        VehicleId = vehicleId;
        LicenseTypeId = licenseTypeId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
