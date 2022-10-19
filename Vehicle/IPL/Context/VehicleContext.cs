using Vehicle.DL.Models;

namespace Vehicle.IPL.Context;
internal class MockVehicleContext
{
    private HashSet<DL.Models.Vehicle> _vehicles;
    public HashSet<DL.Models.Vehicle> Vehicles => _vehicles;

    private HashSet<LicenseType> _licenseTypes;
    public HashSet<LicenseType> LicenseTypes => _licenseTypes;

    private HashSet<VehicleInformation> _vehicleInformation;
    public HashSet<VehicleInformation> VehicleInformation => _vehicleInformation;

    private HashSet<Person> _people;
    public HashSet<Person> People => _people;

    public MockVehicleContext()
    {
        _vehicles = new();
        _people = new();
        _licenseTypes = new();
        _vehicleInformation = new();
    }
}
