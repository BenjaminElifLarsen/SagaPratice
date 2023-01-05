using Common.Events.Domain;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
public sealed record VehicleInformationRemoved : DomainEvent //the code that triggers this should not be able to delete the entity as long time a vehicle reference it
{
    internal VehicleInformationRemoved(VehicleInformation aggregate, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
    }
}
