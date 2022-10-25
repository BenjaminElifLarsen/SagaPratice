using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
public class AddVehicleInformationFromExternalSystem : ICommand
{
    public string VehicleName { get; private set; }
    public int LicenseTypeId { get; private set; }
    public byte MaxNumberOfWheel { get; private set; }
}
