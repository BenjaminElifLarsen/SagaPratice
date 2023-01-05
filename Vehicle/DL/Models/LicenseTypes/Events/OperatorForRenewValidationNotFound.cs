using Common.Events.System;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record OperatorForRenewValidationNotFound : SystemEvent
{
    public Guid LicenseTypeId { get; private set; }
    public Guid OperatorId { get; private set; }

    public OperatorForRenewValidationNotFound(Guid operatorId, Guid licenseTypeId, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}