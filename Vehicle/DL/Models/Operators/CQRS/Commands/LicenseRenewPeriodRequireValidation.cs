using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class LicenseRenewPeriodRequireValidation : ICommand
{
    public Guid CommandId { get; private set; }
    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }
    public int OperatorId { get; private set; }
    public int LicenseTypeId { get; private set; }
    public byte NewRenewPeriod { get; private set; }
    
    public LicenseRenewPeriodRequireValidation(int operatorId, int licenseTypeId, byte newRenewPeriod, Guid correlationId, Guid causationId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
        NewRenewPeriod = newRenewPeriod;
        CorrelationId = correlationId;
        CausationId = causationId;
        CommandId = Guid.NewGuid();
    }
}
