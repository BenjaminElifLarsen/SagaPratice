using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleSold : DomainEvent
{
    public IEnumerable<Guid> OperatorIds { get; private set; }
    internal VehicleSold(Vehicle aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        OperatorIds = aggregate.Operators;
    }
}