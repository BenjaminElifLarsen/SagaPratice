﻿using Common.Events.Domain;
using Common.RepositoryPattern;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;
using VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;

namespace VehicleDomain.DL.Models.Vehicles;
public class Vehicle : IAggregateRoot
{
    /*
     * could have a Wheel model and Vehicle could have a collection of wheels, max amount controlled by MaxWheelAmount in vehicle information
     */
    private int _id;
    private DateTime _productionDate;
    private IdReference<int> _vehicleInformation;
    private double _distanceMovedKm; //this could make use of event sourcing
    private readonly HashSet<IdReference<int>> _operators;
    private bool _inUse;
    private SerielNumber _serielNumber; //ensure uniqueness
    private readonly HashSet<DomainEvent> _events;

    internal DateTime ProductionDate { get => _productionDate; private set => _productionDate = value; }
    internal IdReference<int> VehicleInformation { get => _vehicleInformation; private set => _vehicleInformation = value; }
    internal double DistanceMovedKm { get => _distanceMovedKm; private set => _distanceMovedKm = value; }
    internal IEnumerable<IdReference<int>> Operators => _operators;
    internal bool InUse { get => _inUse; private set => _inUse = value; }
    internal SerielNumber SerielNumber { get => _serielNumber; private set => _serielNumber = value; }

    public int Id { get => _id; private set => _id = value; }

    public IEnumerable<DomainEvent> Events => _events;

    private Vehicle()
    {
        _events = new();
    }

    internal Vehicle(DateTime productionDate, IdReference<int> vehicleInformation, SerielNumber serielNumber)
    {
        _id = RandomValue.GetValue;
        _distanceMovedKm = 0;
        _productionDate = productionDate;
        _vehicleInformation = vehicleInformation;
        _operators = new();
        SerielNumber = serielNumber;
        _events = new();
    }

    internal Vehicle(DateTime productionDate, IdReference<int> vehicleInformation, SerielNumber serielNumber, double distanceMovedKm) : this(productionDate, vehicleInformation, serielNumber)
    {
        _distanceMovedKm = distanceMovedKm;
    }

    internal int UpdateProductionDate(DateTime produced)
    {
        _productionDate = produced;
        return 0;
    }

    //public int ReplaceVehicleInformation(IdReference vehicleInformation)
    //{ //makes more sense to remove and create a new one
    //    _vehicleInformation = vehicleInformation;
    //    return 0;
    //}

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

    internal bool AddOperator(IdReference<int> @operator)
    {
        return _operators.Add(@operator);
    }

    internal bool RemoveOperator(IdReference<int> @operator)
    {
        return _operators.Remove(@operator);
    }

    internal bool IsOperatorPermitted(IdReference<int> @operator)
    {
        return _operators.Any(x => x == @operator);
    }

    internal void StartOperating(IdReference<int> @operator)
    {
        if (Operators.Any(x => x.Id == @operator.Id))
        {
            _inUse = true;
        }
    }

    internal void StopOperating(IdReference<int> @operator)
    {
        if (Operators.Any(x => x.Id == @operator.Id))
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
