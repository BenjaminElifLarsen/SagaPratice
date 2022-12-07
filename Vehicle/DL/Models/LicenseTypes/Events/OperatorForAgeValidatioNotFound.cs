﻿using Common.Events.Domain;
using VehicleDomain.DL.Models.Operators;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
internal class OperatorForAgeValidatioNotFound : IDomainEvent<OperatorForAgeValidatioNotFoundData>
{
    public OperatorForAgeValidatioNotFoundData Data { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public int Version { get; private set; }

    public OperatorForAgeValidatioNotFound(int operatorId, int licenseTypeId, Guid correlationId, Guid causationId)
    {
        AggregateType = typeof(Operator).Name;
        AggregateId = operatorId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Version = 0;
        Data = new(operatorId, licenseTypeId);
    }
}

internal class OperatorForAgeValidatioNotFoundData
{
    public int OperatorId { get; private set; }
    public int LicenseTypeId { get; private set; }

    public OperatorForAgeValidatioNotFoundData(int operatorId, int licenseTypeId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}