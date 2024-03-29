﻿using Common.CQRS.Commands;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL.Handlers.Event;
internal class VehicleEventHandler : IVehicleEventHandler
{
    private readonly IVehicleCommandBus _commandBus;

    public VehicleEventHandler(IVehicleCommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void Handle(LicenseTypeAgeRequirementChanged @event)
    { //will needd to find all licenses that uese the given license type, via their operator, and validate them
        _commandBus.Dispatch(new ValidateLicenseAgeRequirementBecauseChange(@event.AggregateId, @event.NewAgeRequirement, @event.OperatorIds, @event.CorrelationId, @event.EventId)); //needs command handler
    }

    public void Handle(LicenseTypeRenewPeriodChanged @event) //license type do know of the operators that need to be contacted, consider transmitted that data (goes for above too)
    { //will need to find all licenses that use the given license type, via their operator, and validate them
        _commandBus.Dispatch(new ValidateLicenseRenewPeriodBecauseChange(@event.AggregateId, @event.NewRenewPeriodInYears, @event.OperatorIds, @event.CorrelationId, @event.EventId)); //needs command license 
    }

    public void Handle(LicenseTypeRetracted @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(OperatorAdded @event)
    {
        throw new NotImplementedException();
    }

    public void Handle(OperatorGainedLicense @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(OperatorLicenseExpired @event)
    { //not compatible with the command handler. The command handler expect a vehicle id. 
        //but operator does not know which vehicle require which license type.
        //vehicle knows it vehicle information and vehicle information knows it license type. 
        //so the command needs to get the specific vehicle information with the license type id and then get all vehicle with that vehicle information and also got a reference to the operator in their operator collection.
        //in not wanting to operator on multiple aggregates in one command, could create events for each combination
        _commandBus.Dispatch(new RemoveOperatorFromVehicle(@event.AggregateId, @event.LicenseTypeId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(OperatorLicenseRenewed @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(OperatorLicenseRetracted @event)
    {
        _commandBus.Dispatch(new RemoveOperatorFromVehicle(@event.AggregateId, @event.LicenseTypeId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(OperatorRemoved @event)
    {
        foreach(var vehicle in @event.VehicleIds)
        {
            _commandBus.Dispatch(new RemoveOperatorFromVehicle(vehicle, @event.AggregateId, @event.CorrelationId, @event.EventId));
        }
        foreach(var licenseType in @event.LicenseTypeIds)
        {
            _commandBus.Dispatch(new RemoveOperatorFromLicenseType(@event.AggregateId, licenseType, @event.CorrelationId, @event.EventId)); //needs command handler
        }
    }

    public void Handle(VehicleInformationAdded @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(VehicleInformationRemoved @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(VehicleBought @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(VehicleOperatorRelationshipEstablished @event)
    {
        _commandBus.Dispatch(new AddVehicleToOperator(@event.AggregateId, @event.OperatorId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new AddOperatorToVehicle(@event.AggregateId, @event.OperatorId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(VehicleOperatorRelationshipDisbanded @event)
    {
        _commandBus.Dispatch(new RemoveVehicleFromOperator(@event.AggregateId, @event.OperatorId, @event.CorrelationId, @event.EventId));
        _commandBus.Dispatch(new RemoveOperatorFromVehicle(@event.AggregateId, @event.OperatorId, @event.CorrelationId, @event.EventId));
    }

    public void Handle(VehicleSold @event)
    {
        foreach(var @operator in @event.OperatorIds)
        {
            _commandBus.Dispatch(new RemoveVehicleFromOperator(@event.AggregateId, @operator, @event.CorrelationId, @event.EventId));
        }
    }
}
