using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeRenewPeriodChanged : DomainEvent
{
    public byte NewRenewPeriodInYears { get; private set; }

    public IEnumerable<int> OperatorIds { get; private set; }
    
    internal LicenseTypeRenewPeriodChanged(LicenseType aggregate, Guid correlationId, Guid causationId) : base(aggregate.Id, aggregate.GetType().Name, aggregate.Events.Count(), correlationId, causationId)
    {
        NewRenewPeriodInYears = aggregate.RenewPeriodInYears;
        OperatorIds = aggregate.Operators.Select(x => x.Id).ToList();
    }
}