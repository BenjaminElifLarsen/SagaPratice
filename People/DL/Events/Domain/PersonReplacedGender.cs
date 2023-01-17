using Common.Events.Domain;
using Common.Events.Store.Event;
using Common.Events.System;
using PersonDomain.DL.Events.Conversion;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonReplacedGender : SystemEvent
{
    public Guid PersonId { get; private set; }
    public Guid NewGenderId { get; private set; }

    public Guid OldGenderId { get; private set; }

    internal PersonReplacedGender(Person aggregate, Guid oldGenderId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        NewGenderId = aggregate.Gender;
        OldGenderId = oldGenderId;
        PersonId = aggregate.Id;
    }

}