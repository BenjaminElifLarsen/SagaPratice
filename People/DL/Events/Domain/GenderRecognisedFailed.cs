using Common.Events.Domain;

using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed class GenderRecognisedFailed : IDomainEventFail
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }
    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public GenderRecognisedFailed(IEnumerable<string> errors, Guid correlationid, Guid causationId)
    {
        AggregateType = typeof(Gender).Name;
        AggregateId = 0;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationid;
        CausationId = causationId;
        Version = 0;
        Errors = errors;

    }
}
