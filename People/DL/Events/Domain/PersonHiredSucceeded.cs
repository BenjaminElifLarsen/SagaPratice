using Common.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonHiredSucceeded : DomainEvent
{
    public Guid GenderId { get; private set; }

    internal PersonHiredSucceeded(Person aggregate, Guid correlationId, Guid causationId) :
        base(aggregate, correlationId, causationId)
    {
        GenderId = aggregate.Gender;
    }
}