using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed class PersonFiredFailed : IDomainEventFail
{
    public string AggregateType { get; private set; }
    public int AggregateId { get; private set; }
    public string EventType { get; private set; }
    public Guid EventId { get; private set; }
    public long TimeStampRecorded { get; private set; }
    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }
    public int Version { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public PersonFiredFailed(Person aggregate, IEnumerable<string> errors, int version, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Errors = errors;
        Version = version;
    }
}
