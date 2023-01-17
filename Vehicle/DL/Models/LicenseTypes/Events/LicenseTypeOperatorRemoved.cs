using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record LicenseTypeOperatorRemoved : DomainEvent
{ 
    public Guid OperatorId { get; private set; }
    public LicenseTypeOperatorRemoved(LicenseType aggregate, Guid operatorId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorId = operatorId;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}
