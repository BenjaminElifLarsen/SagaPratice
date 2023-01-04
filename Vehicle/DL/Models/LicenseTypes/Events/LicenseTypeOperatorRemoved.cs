using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeOperatorRemoved : DomainEvent
{ 
    public int OperatorId { get; private set; }
    public LicenseTypeOperatorRemoved(LicenseType aggregate, int operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorId = operatorId;
    }
}
