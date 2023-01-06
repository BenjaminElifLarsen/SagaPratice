using Common.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonChangedGender : DomainEvent
{
    public Guid NewGenderId { get; private set; }

    public Guid OldGenderId { get; private set; }

    internal PersonChangedGender(Person aggregate, Guid oldGenderId, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        NewGenderId = aggregate.Gender;
        OldGenderId = oldGenderId;
    }
}