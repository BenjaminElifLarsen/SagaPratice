using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Vehicles;
internal class Vehicle : IAggregateRoot
{
    /*
     * Could have a Wheel model and Vehicle could have a collection of wheels, max amount controlled by MaxWheelAmount in vehicle information
     */
    private int _vehicleId;
    private DateTime _productionDate;
    private IdReference _vehicleInformation;
    private double _distanceMovedKm;
    private HashSet<IdReference> _operators;

    public int VehicleId { get => _vehicleId; private set => _vehicleId = value; }
    public DateTime ProductionDate { get => _productionDate; private set => _productionDate = value; }
    public IdReference VehicleInformation { get => _vehicleInformation; private set => _vehicleInformation = value; }
    public double DistanceMovedKm { get => _distanceMovedKm; private set => _distanceMovedKm = value; }
    public IEnumerable<IdReference> Operators => _operators;

    private Vehicle()
    {

    }

    internal Vehicle(DateTime productionDate, IdReference vehicleInformation)
    {
        _vehicleId = new Random(int.MaxValue).Next();
        _distanceMovedKm = 0;
        _productionDate = productionDate;
        _vehicleInformation = vehicleInformation;
        _operators = new();
    }

    internal Vehicle(int vehicleId, DateTime productionDate, IdReference vehicleInformation) : this(productionDate, vehicleInformation)
    {
        _vehicleId = vehicleId;
    }

    internal Vehicle(int vehicleId, DateTime productionDate, IdReference vehicleInformation, double distanceMovedKm) : this(vehicleId, productionDate, vehicleInformation)
    {
        _distanceMovedKm = distanceMovedKm;
    }

    public int UpdateProductionDate(DateTime produced)
    {
        _productionDate = produced;
        return 0;
    }

    //public int ReplaceVehicleInformation(IdReference vehicleInformation)
    //{ //makes more sense to remove and create a new one
    //    _vehicleInformation = vehicleInformation;
    //    return 0;
    //}

    public void OverwriteDistanceMoved(double newDistance)
    {
        _distanceMovedKm = newDistance;
    }

    public void AddToDistanceMoved(double distanceToAdd)
    {
        _distanceMovedKm += distanceToAdd;
    }

    public bool AddOperator(IdReference @operator)
    {
        return _operators.Add(@operator);
    }

    public bool RemoveOperator(IdReference @operator)
    {
        return _operators.Remove(@operator);
    }

}
