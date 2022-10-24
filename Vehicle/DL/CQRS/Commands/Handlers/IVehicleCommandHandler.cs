using Common.CQRS.Commands;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
internal interface IVehicleCommandHandler :
    ICommandHandler<ValidateDriverLicenseStatus>,
    ICommandHandler<AddPersonNoLicenseFromSystem>,
    ICommandHandler<AddPersonWithLicenseFromUser>,
    ICommandHandler<AddLicenseToPerson>,
    ICommandHandler<EstablishLicenseTypeFromUser>,
    ICommandHandler<RemovePersonFromSystem>,
    ICommandHandler<RemovePersonFromUser>,
    ICommandHandler<ObsoleteLicenseTypeFromUser>,
    ICommandHandler<AlterLicenseType>
{
}
