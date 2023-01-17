using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
public sealed record VehicleInformationAdded : DomainEvent
{
    public VehicleInformationAdded(VehicleInformation aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}
