using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseAgeRequirementValidated : DomainEvent
{
    internal OperatorLicenseAgeRequirementValidated(Operator aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {

    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}
