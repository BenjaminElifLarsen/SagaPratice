using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseRenewed : DomainEvent
{
    public OperatorLicenseRenewed(Operator aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
    }
}
