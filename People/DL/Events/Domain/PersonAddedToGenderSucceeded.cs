using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonAddedToGenderSucceeded : DomainEvent
{
    public int PersonId { get; private set; }

    internal PersonAddedToGenderSucceeded(Gender aggregate, int personId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    { 
        PersonId = personId;
    }
}