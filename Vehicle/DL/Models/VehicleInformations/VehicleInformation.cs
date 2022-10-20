using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.VehicleInformations;
internal class VehicleInformation : IAggregateRoot
{
    private int _vehicleInformationId;
    private string _name;
    private IdReference _licenseTypeRequired;

    public int VehicleInformationId { get => _vehicleInformationId; private set => _vehicleInformationId = value; }

    public VehicleInformation(int vehicleInformationId, string name, IdReference licenseTypeRequired)
    {
        _vehicleInformationId = vehicleInformationId;
        _name = name;
        _licenseTypeRequired = licenseTypeRequired;
    }
}
