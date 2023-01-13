using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
internal sealed record PersonChangedLastName : DomainEvent
{
    public string LastName { get; private set; } 
    public PersonChangedLastName(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
    }

    public PersonChangedLastName(Person aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        LastName = aggregate.LastName;
    }
}
