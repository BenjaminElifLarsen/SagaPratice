﻿using Common.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
public interface IVehicleCommandHandler :
    ICommandHandler<ValidateDriverLicenseStatus>,
    ICommandHandler<AddOperatorNoLicenseFromSystem>,
    ICommandHandler<AddOperatorWithLicenseFromUser>,
    ICommandHandler<AddLicenseToOperator>,
    ICommandHandler<EstablishLicenseTypeFromUser>,
    ICommandHandler<RemoveOperatorFromSystem>,
    ICommandHandler<RemoveOperatorFromUser>,
    ICommandHandler<ObsoleteLicenseTypeFromUser>,
    ICommandHandler<AlterLicenseType>,
    ICommandHandler<AddVehicleInformationFromExternalSystem>,
    ICommandHandler<AddVehicleWithNoOperator>,
    ICommandHandler<AddVehicleWithOperators>,
    ICommandHandler<AddDistanceToVehicleDistance>,
    ICommandHandler<ResetVehicleMovedDistance>,
    ICommandHandler<EstablishRelationBetweenOperatorAndVehicle>,
    ICommandHandler<AddOperatorToVehicle>,
    ICommandHandler<AddVehicleToOperator>,
    ICommandHandler<RemoveRelationBetweenOperatorAndVehicle>,
    ICommandHandler<RemoveOperatorFromVehicle>,
    ICommandHandler<RemoveVehicleFromOperator>
{
}
/*
 * Have a command for adding a vehicle to person and one for adding person to vehicle.
 * Then have a 'composite' command that trigger those two commands. Each command, after all, should only deal with one aggregate
 */