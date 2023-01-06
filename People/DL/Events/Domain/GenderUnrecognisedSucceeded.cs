using Common.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record GenderUnrecognisedSucceeded : DomainEvent
{
    public GenderUnrecognisedSucceeded(Gender aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
    }
}
