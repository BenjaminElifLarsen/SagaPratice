using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record GenderUnrecognisedSucceeded : DomainEvent
{
    public GenderUnrecognisedSucceeded(Gender aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
    }
}
