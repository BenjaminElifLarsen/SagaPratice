using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Events.Conversion;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record GenderRecognisedSucceeded : DomainEvent
{
    public string Subject { get; set; }
    public string Object { get; set; }
    public GenderRecognisedSucceeded(Gender aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        Subject = aggregate.VerbSubject;
        Object = aggregate.VerbObject;
    }

    public GenderRecognisedSucceeded(Event e) : base(e)
    {
        if(e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        Subject = e.Data.Single(x => x == GenderPropertyId.VerbSubject).Value;
        Object = e.Data.Single(x => x == GenderPropertyId.VerbObject).Value;
    }

    public override Event ConvertToEvent()
    {
        return new Event(this, GenderConversion.Get(this), Common.Events.Store.Event.EventType.Create);
    }
}
