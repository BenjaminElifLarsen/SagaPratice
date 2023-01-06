using Common.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonAddedToGenderSucceeded : DomainEvent
{
    public Guid PersonId { get; private set; }

    internal PersonAddedToGenderSucceeded(Gender aggregate, Guid personId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    { 
        PersonId = personId;
    }
}