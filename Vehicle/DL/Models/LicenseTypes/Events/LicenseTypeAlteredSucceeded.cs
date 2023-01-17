using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeAlteredSucceeded : DomainEvent
{ 
    public bool TypeChanged { get; private set; }
    
    public bool AgeRequirementChanged { get; private set; }
    
    public bool RenewPeriodChanged { get; private set; }
    
    public LicenseTypeAlteredSucceeded(LicenseType aggregate, bool typeChanged, bool ageRequirementChanged, bool renewPeriodChanged, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        TypeChanged = typeChanged;
        AgeRequirementChanged = ageRequirementChanged;
        RenewPeriodChanged = renewPeriodChanged;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}