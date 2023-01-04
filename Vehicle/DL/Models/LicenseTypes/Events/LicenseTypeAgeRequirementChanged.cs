using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeAgeRequirementChanged : DomainEvent
{
    public byte NewAgeRequirement { get; private set; }
    
    public IEnumerable<int> OperatorIds { get; private set; }

    internal LicenseTypeAgeRequirementChanged(LicenseType aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
        NewAgeRequirement = aggregate.AgeRequirementInYears;
        OperatorIds = aggregate.Operators.Select(x => x.Id).ToList();
    }
}