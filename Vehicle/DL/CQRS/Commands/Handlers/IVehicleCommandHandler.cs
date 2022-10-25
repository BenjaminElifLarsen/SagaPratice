using Common.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
internal interface IVehicleCommandHandler :
    ICommandHandler<ValidateDriverLicenseStatus>,
    ICommandHandler<AddPersonNoLicenseFromSystem>,
    ICommandHandler<AddPersonWithLicenseFromUser>,
    ICommandHandler<AddLicenseToOperator>,
    ICommandHandler<EstablishLicenseTypeFromUser>,
    ICommandHandler<RemoveOperatorFromSystem>,
    ICommandHandler<RemoveOperatorFromUser>,
    ICommandHandler<ObsoleteLicenseTypeFromUser>,
    ICommandHandler<AlterLicenseType>,
    ICommandHandler<AddVehicleInformationFromExternalSystem>
{
}
/*
 * Have a command for adding a vehicle to person and one for adding person to vehicle.
 * Then have a 'composite' command that trigger those two commands. Each command, after all, should only deal with one aggregate
 */