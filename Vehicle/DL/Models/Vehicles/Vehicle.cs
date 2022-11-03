﻿using Common.RepositoryPattern;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;
using VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;

namespace VehicleDomain.DL.Models.Vehicles;
public class Vehicle : IAggregateRoot
{
    /*
     * Could have a Wheel model and Vehicle could have a collection of wheels, max amount controlled by MaxWheelAmount in vehicle information
     */
    private int _vehicleId;
    private DateTime _productionDate;
    private IdReference _vehicleInformation;
    private double _distanceMovedKm;
    private readonly HashSet<IdReference> _operators;
    private bool _inUse;
    private SerielNumber _serielNumber; //add to vehicle validation, factory and so on.

    internal int VehicleId { get => _vehicleId; private set => _vehicleId = value; }
    internal DateTime ProductionDate { get => _productionDate; private set => _productionDate = value; }
    internal IdReference VehicleInformation { get => _vehicleInformation; private set => _vehicleInformation = value; }
    internal double DistanceMovedKm { get => _distanceMovedKm; private set => _distanceMovedKm = value; }
    internal IEnumerable<IdReference> Operators => _operators;
    internal bool InUse { get => _inUse; private set => _inUse = value; }
    internal SerielNumber SerielNumber { get => _serielNumber; private set => _serielNumber = value; }

    private Vehicle()
    {

    }

    internal Vehicle(DateTime productionDate, IdReference vehicleInformation, SerielNumber serielNumber)
    {
        _vehicleId = RandomValue.GetValue;
        _distanceMovedKm = 0;
        _productionDate = productionDate;
        _vehicleInformation = vehicleInformation;
        _operators = new();
        SerielNumber = serielNumber;
    }

    internal Vehicle(DateTime productionDate, IdReference vehicleInformation, SerielNumber serielNumber, double distanceMovedKm) : this(productionDate, vehicleInformation, serielNumber)
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

    internal int AddToDistanceMoved(double distanceToAdd)
    { //check for overflow. Maybe instead of having the public double, have a ISpecification<Vehicle>
        if (!new IsVehicleDistanceMovedPositiveOrZero().IsSatisfiedBy(distanceToAdd))
        {
            return (int)VehicleErrors.InvalidDistance;
        }
        _distanceMovedKm += distanceToAdd;
        return 0;
    }

    internal bool AddOperator(IdReference @operator)
    {
        return _operators.Add(@operator);
    }

    internal bool RemoveOperator(IdReference @operator)
    {
        return _operators.Remove(@operator);
    }

    internal bool IsOperatorPermitted(IdReference @operator)
    {
        return _operators.Any(x => x == @operator);
    }

    internal void StartOperating(IdReference @operator)
    {
        if (Operators.Any(x => x.Id == @operator.Id))
        {
            _inUse = true;
        }
    }

    internal void StopOperating(IdReference @operator)
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
}
