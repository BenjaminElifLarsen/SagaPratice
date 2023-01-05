using Common.Events.System;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record OperatorForAgeValidatioNotFound : SystemEvent
{

    public Guid OperatorId { get; private set; }
    
    public Guid LicenseTypeId { get; private set; }

    internal OperatorForAgeValidatioNotFound(Guid operatorId, Guid licenseTypeId, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}