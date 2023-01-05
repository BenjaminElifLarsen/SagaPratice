using Common.Events.Domain;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeOperatorRemoved : DomainEvent
{ 
    public Guid OperatorId { get; private set; }
    public LicenseTypeOperatorRemoved(LicenseType aggregate, Guid operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorId = operatorId;
    }
}
