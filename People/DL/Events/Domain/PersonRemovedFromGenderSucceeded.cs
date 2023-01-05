using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonRemovedFromGenderSucceeded : DomainEvent
{
    public Guid PersonId { get; set; }

    internal PersonRemovedFromGenderSucceeded(Gender aggregate, Guid personId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    { 
        PersonId = personId;
    }
}