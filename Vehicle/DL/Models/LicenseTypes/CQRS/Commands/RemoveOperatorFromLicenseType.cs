using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class RemoveOperatorFromLicenseType : ICommand
{
    public int OperatorId { get; private set; }
    public int LicenseTypeId { get; private set; }

    public RemoveOperatorFromLicenseType(int operatorId, int licenseTypeId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}
