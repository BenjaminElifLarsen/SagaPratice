using Common.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Handlers.Command;
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
    ICommandHandler<AddVehicleInformationFromSystem>,
    ICommandHandler<BuyVehicleWithNoOperator>,
    ICommandHandler<BuyVehicleWithOperators>,
    ICommandHandler<AddDistanceToVehicleDistance>,
    ICommandHandler<ResetVehicleMovedDistance>,
    ICommandHandler<EstablishRelationBetweenOperatorAndVehicle>,
    ICommandHandler<AddVehicleToOperator/*, VehicleOperatorRelationshipEstablished*/>,
    ICommandHandler<AddOperatorToVehicle/*, VehicleOperatorRelationshipEstablished*/>,
    ICommandHandler<RemoveRelationBetweenOperatorAndVehicle>,
    ICommandHandler<RemoveVehicleFromOperator/*, VehicleOperatorRelationshipDisbanded*/>,
    ICommandHandler<RemoveOperatorFromVehicle/*, VehicleOperatorRelationshipDisbanded*/>,
    //ICommandHandler<RemoveOperatorFromVehicle/*, OperatorRemoved*/>, //should data contain a collection over all vehicle ids? Or would it be better to create an event for each combination of operatorId and vehicleId?
    //ICommandHandler<RemoveOperatorFromVehicle/*, OperatorLicenseRetracted*/>, //same, but here the command handler should check if the vehicle require the speicific license type id
    //ICommandHandler<RemoveOperatorFromVehicle/*, OperatorLicenseExpired*/>, // ^ 
    ICommandHandler<StartOperatingVehicle>,
    ICommandHandler<StopOperatingVehicle>
{
}
/*
 * Have a command for adding a vehicle to person and one for adding person to vehicle.
 * Then have a 'composite' command that trigger those two commands via events. Each command, after all, should only deal with one aggregate
 */