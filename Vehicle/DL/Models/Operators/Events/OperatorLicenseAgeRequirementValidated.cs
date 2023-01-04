using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseAgeRequirementValidated : DomainEvent
{
    internal OperatorLicenseAgeRequirementValidated(Operator aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {

    }
}
