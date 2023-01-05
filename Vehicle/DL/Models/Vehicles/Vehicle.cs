using Common.Events.Domain;
using Common.RepositoryPattern;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;
using VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;

namespace VehicleDomain.DL.Models.Vehicles;
public class Vehicle : IAggregateRoot
{
    /*
     * could have a Wheel model and Vehicle could have a collection of wheels, max amount controlled by MaxWheelAmount in vehicle information
     */
    private Guid _id;
    private DateTime _productionDate;
    private IdReference _vehicleInformation;
    private double _distanceMovedKm; //this could make use of event sourcing
    private readonly HashSet<IdReference> _operators;
    private bool _inUse;
    private SerielNumber _serielNumber; //ensure uniqueness
    private readonly HashSet<DomainEvent> _events;

    internal DateTime ProductionDate { get => _productionDate; private set => _productionDate = value; }
    internal Guid VehicleInformation { get => _vehicleInformation; private set => _vehicleInformation = value; }
    internal double DistanceMovedKm { get => _distanceMovedKm; private set => _distanceMovedKm = value; }
    internal IEnumerable<Guid> Operators => _operators.Select(x => x.Id);
    internal bool InUse { get => _inUse; private set => _inUse = value; }
    internal SerielNumber SerielNumber { get => _serielNumber; private set => _serielNumber = value; }

    public Guid Id { get => _id; private set => _id = value; }

    public IEnumerable<DomainEvent> Events => _events;

    private Vehicle()
    {
        _events = new();
    }

    internal Vehicle(DateTime productionDate, Guid vehicleInformation, SerielNumber serielNumber)
    {
        _id = Guid.NewGuid();
        _distanceMovedKm = 0;
        _productionDate = productionDate;
        _vehicleInformation = vehicleInformation;
        _operators = new();
        SerielNumber = serielNumber;
        _events = new();
    }

    internal Vehicle(DateTime productionDate, Guid vehicleInformation, SerielNumber serielNumber, double distanceMovedKm) : this(productionDate, vehicleInformation, serielNumber)
    {
        _distanceMovedKm = distanceMovedKm;
    }

    internal int UpdateProductionDate(DateTime produced)
    {
        _productionDate = produced;
        return 0;
    }

    internal int OverwriteDistanceMoved(double newDistance)
    {
        if(!new IsVehicleDistanceMovedPositiveOrZero().IsSatisfiedBy(newDistance))
        {
            return (int)VehicleErrors.InvalidDistance;
        }
        _distanceMovedKm = newDistance;
        return 0;
    }

    internal int AddToDistanceMoved(double distanceToAdd) //this code would be triggered by an automated system that gets the actually distance from the vehicle automatic.
    { //should check for overflow.
        if (!new IsVehicleDistanceMovedPositiveOrZero().IsSatisfiedBy(distanceToAdd))
        { //this code could cause problems with the model purity. 
            return (int)VehicleErrors.InvalidDistance;
        }
        _distanceMovedKm += distanceToAdd;
        return 0;
    }

    internal bool AddOperator(Guid @operator)
    {
        return _operators.Add(@operator);
    }

    internal bool RemoveOperator(Guid @operator)
    {
        return _operators.Remove(@operator);
    }

    internal bool IsOperatorPermitted(Guid @operator)
    {
        return _operators.Any(x => x == @operator);
    }

    internal void StartOperating(Guid @operator)
    {
        if (Operators.Any(x => x == @operator))
        {
            _inUse = true;
        }
    }

    internal void StopOperating(Guid @operator)
    {
        if (Operators.Any(x => x == @operator))
        { 
            _inUse = false;
        }
    }

    internal void ReplaceSerielNumber(SerielNumber serielNumber)
    {
        _serielNumber = serielNumber;
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        if (_id == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        if (_id == eventItem.AggregateId) //should cause an expection if this fails
            _events.Remove(eventItem);
    }
}
