using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonFiredSucceeded : DomainEvent
{
    public Guid GenderId { get; private set; }
    public DateOnly DeletedFrom { get; private set; }

    internal PersonFiredSucceeded(Person aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        GenderId = aggregate.Gender;
        DeletedFrom = aggregate.DeletedFrom.Value;
    }

    public PersonFiredSucceeded(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        GenderId = Guid.Parse(e.Data.Single(x => x == PersonPropertyId.Gender).Value);
        var deleteDateTime = new DateTime(long.Parse(e.Data.Single(x => x == PersonPropertyId.DeletedFrom).Value));
        DeletedFrom = new(deleteDateTime.Year,deleteDateTime.Month,deleteDateTime.Day);
    }
}