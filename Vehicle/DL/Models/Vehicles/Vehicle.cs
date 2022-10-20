using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Vehicles;
internal class Vehicle : IAggregateRoot
{
    private int _vehicleId;
    private DateTime _produced;
    private IdReference _vehicleInformation;
    private double _distanceDrivenKm;

    public int VehicleId { get => _vehicleId; private set => _vehicleId = value; }
    public DateTime Produced { get => _produced; private set => _produced = value; }
    public IdReference VehicleInformation { get => _vehicleInformation; private set => _vehicleInformation = value; }
    public double DistanceDrivenKm { get => _distanceDrivenKm; private set => _distanceDrivenKm = value; }

    private Vehicle()
    {

    }

    internal Vehicle(int vehicleId, DateTime produced, IdReference vehicleInformation)
    {
        _vehicleId = vehicleId;
        _produced = produced;
        _vehicleInformation = vehicleInformation;
        _distanceDrivenKm = 0;
    }

    internal Vehicle(int vehicleId, DateTime produced, IdReference vehicleInformation, double distanceDrivenKm) : this(vehicleId, produced, vehicleInformation)
    {
        _distanceDrivenKm = distanceDrivenKm;
    }

    public void UpdateProductionDate(DateTime produced)
    {
        _produced = produced;
    }

    public void ReplaceVehicleInformation(IdReference vehicleInformation)
    {
        _vehicleInformation = vehicleInformation;
    }

    public void OverwriteDistanceDriven(double newDistance)
    {
        _distanceDrivenKm = newDistance;
    }

    public void AddToDistanceDriven(double distanceToAdd)
    {
        _distanceDrivenKm += distanceToAdd;
    }


}
