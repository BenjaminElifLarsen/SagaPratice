using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
public class FindVehicleInformationsWithSpecificLicenseType : ICommand
{
    public Guid CommandId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }
    public Guid LicenseTypeId { get; private set; }
    public Guid OperatorId { get; private set; }

    public FindVehicleInformationsWithSpecificLicenseType(Guid operatorId, Guid licenseTypeId, Guid correlationId, Guid causationId)
    {
        LicenseTypeId = licenseTypeId;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
        OperatorId = operatorId;
    }
}
