using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonChangedGender : DomainEvent
{
    public Guid GenderId { get; private set; }
    public PersonChangedGender(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        GenderId = Guid.Parse(e.Data.Single(x => x == PersonPropertyId.Gender).Value);
    }

    public PersonChangedGender(Person aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        GenderId = aggregate.Gender;
    }
}
