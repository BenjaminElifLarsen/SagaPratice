using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeAlteredSucceeded : DomainEvent
{ 
    public bool TypeChanged { get; private set; }
    
    public bool AgeRequirementChanged { get; private set; }
    
    public bool RenewPeriodChanged { get; private set; }
    
    public LicenseTypeAlteredSucceeded(LicenseType aggregate, bool typeChanged, bool ageRequirementChanged, bool renewPeriodChanged, Guid correlationId, Guid causationId)
        : base(aggregate.Id, aggregate.GetType().Name, aggregate.Events.Count(), correlationId, causationId)
    {
        TypeChanged = typeChanged;
        AgeRequirementChanged = ageRequirementChanged;
        RenewPeriodChanged = renewPeriodChanged;
    }
}