using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonRemovedFromGenderSucceeded : DomainEvent
{
    public int PersonId { get; set; }

    internal PersonRemovedFromGenderSucceeded(Gender aggregate, int personId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    { 
        PersonId = personId;
    }
}