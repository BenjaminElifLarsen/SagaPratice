using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
public class FindVehicleInformationsWithSpecificLicenseType : ICommand
{
    public Guid CommandId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }
    public int LicenseTypeId { get; private set; }
    public int OperatorId { get; private set; }

    public FindVehicleInformationsWithSpecificLicenseType(int operatorId, int licenseTypeId, Guid correlationId, Guid causationId)
    {
        LicenseTypeId = licenseTypeId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
        OperatorId = operatorId;
    }
}
