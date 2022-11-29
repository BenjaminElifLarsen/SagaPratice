﻿using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public class GenderRecognised : IDomainEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public GenderRecognised(Gender aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.GenderId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
    }
}