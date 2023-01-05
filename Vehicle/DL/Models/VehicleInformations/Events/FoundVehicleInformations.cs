using Common.Events.System;

namespace VehicleDomain.DL.Models.VehicleInformations.Events;
public sealed record FoundVehicleInformations : SystemEvent
{

    public Guid OperatorId { get; private set; }
    
    public IEnumerable<Guid> VehicleInformationIds { get; private set; }

    internal FoundVehicleInformations(Guid operatorId, IEnumerable<Guid> vehicleInformationIds, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        VehicleInformationIds = vehicleInformationIds;
    }
}