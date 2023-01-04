using Common.Events.System;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
public sealed record OperatorForRenewValidationNotFound : SystemEvent
{
    public int LicenseTypeId { get; private set; }
    public int OperatorId { get; private set; }

    public OperatorForRenewValidationNotFound(int operatorId, int licenseTypeId, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        LicenseTypeId = licenseTypeId;
    }
}