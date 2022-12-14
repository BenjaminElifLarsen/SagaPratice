﻿using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public sealed class PersonFiredSucceeded : IDomainEvent<PersonFiredData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public PersonFiredData Data { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal PersonFiredSucceeded(Person aggregate, int version, Guid correlationId, Guid causationId)
    { //instead of PersonData data, could just create an instance out of Person.
        //domain events are supposed to be triggered before saving, which means an ORM cannot assign an Id, so will need to do that 'manual'.
        //mayhaps factory or handler should assign an id and validate it against the context?
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = version;
        Data = new(aggregate.PersonId, aggregate.Gender.Id);
    }
}

public class PersonFiredData
{
    public int PersonId { get; private set; }
    public int GenderId { get; private set; }

    public PersonFiredData(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}