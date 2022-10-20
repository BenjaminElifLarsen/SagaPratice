using Common.ResultPattern;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
internal class VehicleCommandHandler : IVehicleCommandHandler
{
    public Result Handle(ValidateDriverLicenseStatus command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(AddPersonNoLicenseFromSystem command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(AddPersonWithLicenseFromUser command)
    {
        throw new NotImplementedException();
    }
}
