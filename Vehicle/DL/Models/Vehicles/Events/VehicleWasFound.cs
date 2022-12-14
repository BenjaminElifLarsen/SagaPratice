﻿using Common.Events.Domain;
using Common.Events.Store;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public class VehicleWasFound : IDomainEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    internal VehicleWasFound(Vehicle aggregate, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.VehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = aggregate.Events.Count();
    }

    internal VehicleWasFound(int vehicleId, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = vehicleId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
    }
}
