using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorGainedLicense : DomainEvent
{
    public Guid LicenseTypeId { get; set; }
    internal OperatorGainedLicense(Operator aggregate, int licenseId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        LicenseTypeId = aggregate.Licenses.SingleOrDefault(x => x.LicenseId == licenseId).Type;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}