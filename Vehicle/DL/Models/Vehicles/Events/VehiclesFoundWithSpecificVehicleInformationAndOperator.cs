using Common.Events.Domain;
using Common.Events.System;

namespace VehicleDomain.DL.Models.Vehicles.Events;
public sealed record VehiclesFoundWithSpecificVehicleInformationAndOperator : SystemEvent
{

    public int OperatorId { get; private set; }
    
    public IEnumerable<int> VehicleIds { get; private set; }

    internal VehiclesFoundWithSpecificVehicleInformationAndOperator(int operatorId, IEnumerable<int> vehicleIds, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        OperatorId = operatorId;
        VehicleIds = vehicleIds;
    }
}