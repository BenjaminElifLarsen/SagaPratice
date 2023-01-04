﻿using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehicleNotFound : SystemEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public IEnumerable<string> Errors { get; private set; }

    internal VehicleNotFound(int id, IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Vehicle).Name;
        AggregateId = id;
        Errors = errors;
    }
}
