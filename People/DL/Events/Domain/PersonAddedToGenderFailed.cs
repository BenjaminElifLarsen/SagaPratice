using Common.Events.System;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed record PersonAddedToGenderFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public Guid GenderId { get; private set; }
    public Guid PersonId { get; private set; }

    internal PersonAddedToGenderFailed(Guid personId, Guid genderId, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        Errors = errors;
        GenderId = genderId;
        PersonId = personId;
    }
}
