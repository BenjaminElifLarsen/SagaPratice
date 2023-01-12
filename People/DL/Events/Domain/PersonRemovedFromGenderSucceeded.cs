using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonRemovedFromGenderSucceeded : DomainEvent
{
    public Guid PersonId { get; set; }

    internal PersonRemovedFromGenderSucceeded(Gender aggregate, Guid personId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    { 
        PersonId = personId;
    }

    public PersonRemovedFromGenderSucceeded(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        PersonId = Guid.Parse(e.Data.SingleOrDefault(x => x == GenderPropertyId.PersonId).Value);
    }
}