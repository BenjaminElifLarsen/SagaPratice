using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Conversion;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record GenderUnrecognisedSucceeded : DomainEvent
{
    public GenderUnrecognisedSucceeded(Gender aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
    }

    public GenderUnrecognisedSucceeded(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
    }

    public override Event ConvertToEvent()
    {
        return new Event(this, GenderConversion.Get(this), Common.Events.Store.Event.EventType.Remove);
    }
}
