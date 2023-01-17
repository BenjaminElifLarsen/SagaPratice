using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeRenewPeriodChanged : DomainEvent
{
    public byte NewRenewPeriodInYears { get; private set; }

    public IEnumerable<Guid> OperatorIds { get; private set; }
    
    internal LicenseTypeRenewPeriodChanged(LicenseType aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
        NewRenewPeriodInYears = aggregate.RenewPeriodInYears;
        OperatorIds = aggregate.Operators;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}