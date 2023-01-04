using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record GenderRecognisedSucceeded : DomainEvent
{
    public string Subject { get; set; }
    public string Object { get; set; }
    public GenderRecognisedSucceeded(Gender aggregate, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.Id;
        Version = aggregate.Events.Count();
        Subject = aggregate.VerbSubject;
        Object = aggregate.VerbObject;
    }
}
