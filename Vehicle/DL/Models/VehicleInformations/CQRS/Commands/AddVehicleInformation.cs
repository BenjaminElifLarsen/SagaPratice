using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
public class AddVehicleInformationFromSystem : ICommand
{
    public string VehicleName { get; set; }
    public int LicenseTypeId { get; set; }
    public byte MaxNumberOfWheel { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public AddVehicleInformationFromSystem()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    public AddVehicleInformationFromSystem(string vehicleName, int licenseTypeId, byte maxWheelAmount, Guid correlationId, Guid causationId)
    {
        CommandId = Guid.NewGuid();
        VehicleName = vehicleName;
        LicenseTypeId = licenseTypeId;
        MaxNumberOfWheel = maxWheelAmount;
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}
