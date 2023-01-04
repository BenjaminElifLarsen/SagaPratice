using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonChangedGender : DomainEvent
{
    public int NewGenderId { get; private set; }

    public int OldGenderId { get; private set; }

    internal PersonChangedGender(Person aggregate, int oldGenderId, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        NewGenderId = aggregate.Gender.Id;
        OldGenderId = oldGenderId;
    }
}