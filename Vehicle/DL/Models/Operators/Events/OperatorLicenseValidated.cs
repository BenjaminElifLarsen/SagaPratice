using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseValidated : DomainEvent
{
    public OperatorLicenseValidated(Operator aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
    }
}
