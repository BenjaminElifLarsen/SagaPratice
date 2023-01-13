﻿using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;

namespace PersonDomain.DL.Events.Domain;
internal sealed record PersonChangedFirstName : DomainEvent
{
    public string FirstName { get; private set; }
    public PersonChangedFirstName(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        FirstName = e.Data.Single(x => x == PersonPropertyId.FirstName).Value;
    }

    public PersonChangedFirstName(Person aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        FirstName = aggregate.FirstName;
    }
}
