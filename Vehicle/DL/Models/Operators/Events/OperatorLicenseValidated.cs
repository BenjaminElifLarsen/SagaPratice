﻿using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
internal class OperatorLicenseValidated : IDomainEvent
{
    public string AggregateType => throw new NotImplementedException();

    public int AggregateId => throw new NotImplementedException();

    public string EventType => throw new NotImplementedException();

    public Guid EventId => throw new NotImplementedException();

    public long TimeStampRecorded => throw new NotImplementedException();

    public Guid CorrelationId => throw new NotImplementedException();

    public Guid CausationId => throw new NotImplementedException();

    public int Version => throw new NotImplementedException();
}