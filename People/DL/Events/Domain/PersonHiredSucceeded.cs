using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Conversion;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonHiredSucceeded : DomainEvent
{
    public Guid GenderId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime Birth { get; private set; }

    internal PersonHiredSucceeded(Person aggregate, Guid correlationId, Guid causationId) :
        base(aggregate, correlationId, causationId)
    {
        GenderId = aggregate.Gender;
        FirstName = aggregate.FirstName;
        LastName = aggregate.LastName;
        Birth = new(aggregate.Birth.Year, aggregate.Birth.Month, aggregate.Birth.Day)
;    }

    public PersonHiredSucceeded(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        GenderId = Guid.Parse(e.Data.Single(x => x == PersonPropertyId.Gender).Value);
        FirstName = e.Data.Single(x => x == PersonPropertyId.FirstName).Value;
        LastName = e.Data.Single(x => x == PersonPropertyId.LastName).Value;
        Birth = new(long.Parse(e.Data.Single(x => x == PersonPropertyId.Birth).Value));
    }

    public override Event ConvertToEvent()
    {
        return new Event(this, PersonConversion.Get(this), Common.Events.Store.Event.EventType.Create);
    }
}