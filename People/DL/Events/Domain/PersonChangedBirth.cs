using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Conversion;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
internal sealed record PersonChangedBirth : DomainEvent
{
    public DateTime Birth { get; private set; }
    public PersonChangedBirth(Person aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        Birth = new(aggregate.Birth.Year, aggregate.Birth.Month, aggregate.Birth.Day);
    }

    public PersonChangedBirth(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        Birth = new DateTime(long.Parse(e.Data.Single(x => x == PersonPropertyId.Birth).Value));
    }

    public override Event ConvertToEvent()
    {
        return new Event(this, PersonConversion.Get(this), Common.Events.Store.Event.EventType.Modify);
    }
}
