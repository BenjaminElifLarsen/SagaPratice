using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class AddLicenseToOperator : ICommand
{
    public int OperatorId { get; set; }
    public DateTime Arquired { get; set; }
    public int LicenseType { get; set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public AddLicenseToOperator()
    {
        CommandId = Guid.NewGuid();
        CorrelationId = CommandId;
        CausationId = CommandId;
    }

    public AddLicenseToOperator(int operatorId, DateTime arquired, int licenseType, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        Arquired = arquired;
        LicenseType = licenseType;
        CommandId = Guid.NewGuid();
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}
