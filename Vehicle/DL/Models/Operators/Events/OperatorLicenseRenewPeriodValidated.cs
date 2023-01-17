using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseRenewPeriodValidated : DomainEvent
{
    internal OperatorLicenseRenewPeriodValidated(Operator aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {

    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}
