using Common.CQRS.Commands;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
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
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(LicenseTypeRenewPeriodChanged @event)
    {
        throw new NotImplementedException(); //needs command    
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
        _commandBus.Publish(new RemoveOperatorFromVehicle(@event.Data.OperatorId, @event.Data.LicenseTypeId));
    }

    public void Handle(OperatorLicenseRenewed @event)
    {
        throw new NotImplementedException(); //needs command
    }

    public void Handle(OperatorLicenseRetracted @event)
    {
        _commandBus.Publish(new RemoveOperatorFromVehicle(@event.Data.OperatorId, @event.Data.LicenseTypeId));
    }

    public void Handle(OperatorRemoved @event)
    {
        foreach(var vehicle in @event.Data.VehicleIds)
        {
            _commandBus.Publish(new RemoveOperatorFromVehicle(vehicle, @event.Data.Id));
        }
        foreach(var licenseType in @event.Data.LicenseTypeIds)
        {
            _commandBus.Publish(new RemoveOperatorFromLicenseType(@event.Data.Id, licenseType)); //needs command handler
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
        _commandBus.Publish(new AddVehicleToOperator(@event.Data.VehicleId, @event.Data.OperatorId));
        _commandBus.Publish(new AddOperatorToVehicle(@event.Data.VehicleId, @event.Data.OperatorId));
    }

    public void Handle(VehicleOperatorRelationshipDisbanded @event)
    {
        _commandBus.Publish(new RemoveVehicleFromOperator(@event.Data.VehicleId, @event.Data.OperatorId));
        _commandBus.Publish(new RemoveOperatorFromVehicle(@event.Data.VehicleId, @event.Data.OperatorId));
    }

    public void Handle(VehicleSold @event)
    {
        foreach(var @operator in @event.Data.OperatorIds)
        {
            _commandBus.Publish(new RemoveVehicleFromOperator(@event.Data.Id, @operator));
        }
    }
}
