using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleStopped : DomainEvent
{
    internal VehicleStopped(Vehicle aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
    
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}
