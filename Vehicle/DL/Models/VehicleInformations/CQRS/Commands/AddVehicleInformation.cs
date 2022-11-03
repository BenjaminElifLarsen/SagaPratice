using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
public class AddVehicleInformationFromSystem : ICommand
{
    public string VehicleName { get; set; }
    public int LicenseTypeId { get; set; }
    public byte MaxNumberOfWheel { get; set; }
}
