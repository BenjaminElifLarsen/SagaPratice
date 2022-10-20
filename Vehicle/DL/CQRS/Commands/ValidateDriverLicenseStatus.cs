using Common.CQRS.Commands;

namespace VehicleDomain.DL.CQRS.Commands;
public class ValidateDriverLicenseStatus : ICommand
{
    public int OwnerId { get; private set; }
    public int TypeId { get; private set; }
}
