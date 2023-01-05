using Common.Events.Domain;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorGainedLicense : DomainEvent
{
    public int LicenseTypeId { get; set; }
    internal OperatorGainedLicense(Operator aggregate, int licenseId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        LicenseTypeId = aggregate.Licenses.SingleOrDefault(x => x.LicenseId == licenseId).Type.Id;
    }
}