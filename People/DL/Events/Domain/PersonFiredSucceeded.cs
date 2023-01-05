using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonFiredSucceeded : DomainEvent
{
    public Guid GenderId { get; private set; }

    internal PersonFiredSucceeded(Person aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        GenderId = aggregate.Gender;
    }
}