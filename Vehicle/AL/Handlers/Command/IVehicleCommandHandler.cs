using Common.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.AL.Handlers.Command;
public interface IVehicleCommandHandler :
    ICommandHandler<ValidateOperatorLicenseStatus>,
    ICommandHandler<AddOperatorNoLicenseFromSystem>,
    ICommandHandler<AddLicenseToOperator>,
    ICommandHandler<EstablishLicenseTypeFromUser>,
    ICommandHandler<RemoveOperatorFromSystem>,
    ICommandHandler<ObsoleteLicenseTypeFromUser>,
    ICommandHandler<AlterLicenseType>,
    ICommandHandler<AddVehicleInformationFromSystem>,
    ICommandHandler<BuyVehicleWithNoOperator>,
    ICommandHandler<BuyVehicleWithOperators>,
    ICommandHandler<AddDistanceToVehicleDistance>,
    ICommandHandler<ResetVehicleMovedDistance>,
    ICommandHandler<EstablishRelationBetweenOperatorAndVehicle>,
    ICommandHandler<AddVehicleToOperator>,
    ICommandHandler<AddOperatorToVehicle>,
    ICommandHandler<RemoveRelationBetweenOperatorAndVehicle>,
    ICommandHandler<RemoveVehicleFromOperator>,
    ICommandHandler<RemoveOperatorFromVehicle>,
    ICommandHandler<AttemptToStartVehicle>,
    ICommandHandler<StopOperatingVehicle>,
    ICommandHandler<RemoveOperatorFromLicenseType>,
    ICommandHandler<ValidateLicenseAgeRequirementBecauseChange>,
    ICommandHandler<ValidateLicenseRenewPeriodBecauseChange>,
    ICommandHandler<LicenseAgeRequirementRequireValidation>,
    ICommandHandler<LicenseRenewPeriodRequireValidation>,
    ICommandHandler<RemoveOperator>,
    ICommandHandler<FindVehicleInformationsWithSpecificLicenseType>,
    ICommandHandler<FindVehiclesWithSpecificVehicleInformationAndOperator>
{
}