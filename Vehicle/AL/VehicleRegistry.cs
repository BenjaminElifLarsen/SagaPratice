﻿using Common.CQRS.Commands;
using Common.Events.Domain;
using Common.Other;
using VehicleDomain.AL.Busses.Command;
using VehicleDomain.AL.Busses.Event;
using VehicleDomain.AL.Handlers.Command;
using VehicleDomain.AL.Handlers.Event;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Events;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.Events;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.Events;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.Events;

namespace VehicleDomain.AL;
public class VehicleRegistry : IRoutingRegistry
{
    private readonly IVehicleCommandBus _commandBus;
    private readonly IVehicleDomainEventBus _eventBus;
    private readonly IVehicleCommandHandler _commandHandler;
    private readonly IVehicleEventHandler _eventHandler;

    public VehicleRegistry(IVehicleCommandBus commandBus, IVehicleDomainEventBus eventBus, IVehicleCommandHandler commandHandler, IVehicleEventHandler eventHandler)
    {
        _commandBus = commandBus;
        _eventBus = eventBus;
        _commandHandler = commandHandler;
        _eventHandler = eventHandler;
    }

    public void SetUpRouting()
    {
        RoutingCommand();
        RoutingEvent();
    }

    private void RoutingEvent()
    {
        _eventBus.RegisterHandler<LicenseTypeAgeRequirementChanged>(_eventHandler.Handle);
        _eventBus.RegisterHandler<LicenseTypeRenewPeriodChanged>(_eventHandler.Handle);
        _eventBus.RegisterHandler<LicenseTypeRetracted>(_eventHandler.Handle);
        _eventBus.RegisterHandler<OperatorAdded>(_eventHandler.Handle);
        _eventBus.RegisterHandler<OperatorGainedLicense>(_eventHandler.Handle);
        _eventBus.RegisterHandler<OperatorLicenseExpired>(_eventHandler.Handle);
        _eventBus.RegisterHandler<OperatorLicenseRenewed>(_eventHandler.Handle);
        _eventBus.RegisterHandler<OperatorLicenseRetracted>(_eventHandler.Handle);
        _eventBus.RegisterHandler<OperatorRemoved>(_eventHandler.Handle);
        _eventBus.RegisterHandler<VehicleInformationAdded>(_eventHandler.Handle);
        _eventBus.RegisterHandler<VehicleInformationRemoved>(_eventHandler.Handle);
        _eventBus.RegisterHandler<VehicleBought>(_eventHandler.Handle);
        _eventBus.RegisterHandler<VehicleOperatorRelationshipEstablished>(_eventHandler.Handle);
        _eventBus.RegisterHandler<VehicleOperatorRelationshipDisbanded>(_eventHandler.Handle);
        _eventBus.RegisterHandler<VehicleSold>(_eventHandler.Handle);
    }

    private void RoutingCommand()
    {
        _commandBus.RegisterHandler<ValidateDriverLicenseStatus>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddOperatorNoLicenseFromSystem>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddOperatorWithLicenseFromUser>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddLicenseToOperator>(_commandHandler.Handle);
        _commandBus.RegisterHandler<EstablishLicenseTypeFromUser>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemoveOperatorFromSystem>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemoveOperatorFromUser>(_commandHandler.Handle);
        _commandBus.RegisterHandler<ObsoleteLicenseTypeFromUser>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AlterLicenseType>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddVehicleInformationFromSystem>(_commandHandler.Handle);
        _commandBus.RegisterHandler<BuyVehicleWithNoOperator>(_commandHandler.Handle);
        _commandBus.RegisterHandler<BuyVehicleWithOperators>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddDistanceToVehicleDistance>(_commandHandler.Handle);
        _commandBus.RegisterHandler<ResetVehicleMovedDistance>(_commandHandler.Handle);
        _commandBus.RegisterHandler<EstablishRelationBetweenOperatorAndVehicle>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddVehicleToOperator>(_commandHandler.Handle);
        _commandBus.RegisterHandler<AddOperatorToVehicle>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemoveRelationBetweenOperatorAndVehicle>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemoveVehicleFromOperator>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemoveOperatorFromVehicle>(_commandHandler.Handle);
        _commandBus.RegisterHandler<StartOperatingVehicle>(_commandHandler.Handle);
        _commandBus.RegisterHandler<StopOperatingVehicle>(_commandHandler.Handle);
        _commandBus.RegisterHandler<RemoveOperatorFromLicenseType>(_commandHandler.Handle);
    }
}
