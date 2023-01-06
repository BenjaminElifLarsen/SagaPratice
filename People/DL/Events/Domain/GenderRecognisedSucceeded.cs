using Common.Events.Domain;
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
}
