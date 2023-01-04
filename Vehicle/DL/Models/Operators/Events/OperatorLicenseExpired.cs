using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseExpired : DomainEvent
{
    public int LicenseTypeId { get; private set; }
    
    internal OperatorLicenseExpired(Operator aggregate, int licenseTypeId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        LicenseTypeId = licenseTypeId;
    }
}