using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseRenewPeriodValidated : DomainEvent
{
    internal OperatorLicenseRenewPeriodValidated(Operator aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {

    }
}
