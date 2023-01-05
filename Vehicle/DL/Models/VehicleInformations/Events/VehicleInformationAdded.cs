using Common.Events.Domain;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
public sealed record VehicleInformationAdded : DomainEvent
{
    public VehicleInformationAdded(VehicleInformation aggregate, Guid correlationId, Guid causationId) 
        : base(aggregate, correlationId, causationId)
    {
    }
}
