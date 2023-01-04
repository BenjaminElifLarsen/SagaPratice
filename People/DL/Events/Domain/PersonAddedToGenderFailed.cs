using Common.Events.System;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonAddedToGenderFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public int GenderId { get; private set; }
    public int PersonId { get; private set; }

    internal PersonAddedToGenderFailed(int personId, int genderId, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Gender).Name;
        AggregateId = 0;
        Errors = errors;
        GenderId = genderId;
        PersonId = personId;
    }
}
