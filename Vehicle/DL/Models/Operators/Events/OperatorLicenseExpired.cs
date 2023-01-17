using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.Operators.Events;
public sealed record OperatorLicenseExpired : DomainEvent
{
    public Guid LicenseTypeId { get; private set; }
    
    internal OperatorLicenseExpired(Operator aggregate, Guid licenseTypeId, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        LicenseTypeId = licenseTypeId;
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}