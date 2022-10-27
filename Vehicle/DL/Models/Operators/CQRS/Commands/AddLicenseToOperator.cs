using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
internal class AddLicenseToOperator : ICommand
{
    public int OperatorId { get; private set; }
    public DateTime Arquired { get; private set; }
    public int LicenseType { get; private set; }
}
