using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehiclesFoundWithSpecificVehicleInformationAndOperator : SystemEvent
{

    public Guid OperatorId { get; private set; }
    
    public IEnumerable<Guid> VehicleIds { get; private set; }

    internal VehiclesFoundWithSpecificVehicleInformationAndOperator(Guid operatorId, IEnumerable<Guid> vehicleIds, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        VehicleIds = vehicleIds;
    }
}