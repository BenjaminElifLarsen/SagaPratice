using Common.Events.Domain;
using Common.Events.System;
using VehicleDomain.DL.Models.Operators;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record OperatorForAgeValidatioNotFound : SystemEvent
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public int OperatorId { get; private set; }
    
    public int LicenseTypeId { get; private set; }

    internal OperatorForAgeValidatioNotFound(int operatorId, int licenseTypeId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Operator).Name;
        AggregateId = operatorId;
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}