using Common.Events.Domain;
using Common.Events.Store.Event;

namespace VehicleDomain.DL.Models.LicenseTypes.Events;
/// <summary>
/// The license type has been retracted and thus license using it are no longer valid.
/// </summary>
public sealed record LicenseTypeRetracted : DomainEvent
{ // License Type has a ISoftDeleteFrom contract, so this event should first be published when that date has been reached.

    internal LicenseTypeRetracted(LicenseType aggregate, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
    }

    public override Event ConvertToEvent()
    {
        throw new NotImplementedException();
    }
}