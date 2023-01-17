using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleSold : DomainEvent
{
    public IEnumerable<Guid> OperatorIds { get; private set; }
    internal VehicleSold(Vehicle aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorIds = aggregate.Operators;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}