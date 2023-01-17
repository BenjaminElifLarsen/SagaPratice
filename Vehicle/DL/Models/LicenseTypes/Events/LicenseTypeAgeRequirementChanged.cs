using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeAgeRequirementChanged : DomainEvent
{
    public byte NewAgeRequirement { get; private set; }
    
    public IEnumerable<Guid> OperatorIds { get; private set; }

    internal LicenseTypeAgeRequirementChanged(LicenseType aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
        NewAgeRequirement = aggregate.AgeRequirementInYears;
        OperatorIds = aggregate.Operators;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}