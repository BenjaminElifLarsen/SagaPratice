using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorAdded : DomainEvent
{ //is this event needed? What would react to it in this domain? Really just here to store it in event store
    public OperatorAdded(IAggregateRoot aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
    }
}
